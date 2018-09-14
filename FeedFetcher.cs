using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace PodPuppy
{
    public class FeedFetcher
    {
        public const int ReadBufferLengthBytes = 1024;

        private bool _busy;
        private bool _canceled;
        private string _url;
        private WebRequest _webRequest;
        private OnFeedFetchedHandler _onFeedFetched;

        private FeedFetcher _redirectFetcher;

        private static Regex _altLinkRgx = null;
        private static Regex _altLinkURLRgx = null;

        public delegate void OnFeedFetchedHandler(FeedRefreshResult status, XmlDocument feed, string errorMsg, string url);

        static FeedFetcher()
        {
            _altLinkRgx = new Regex("<link(.+?)>", RegexOptions.Compiled);
            _altLinkURLRgx = new Regex("href=\"(.+?)\"");
        }

        public FeedFetcher()
        {
            _busy = false;
        }

        public bool IsBusy
        {
            get { return _busy; }
        }

        public bool Fetch(string url, OnFeedFetchedHandler onFeedFetched)
        {
            return Fetch(url, onFeedFetched, true);
        }

        private bool Fetch(string url, OnFeedFetchedHandler onFeedFetched, bool externalCall)
        {
            if (externalCall && _busy)
                return false;
            _busy = true;

            _url = url;

            _canceled = false;
            _onFeedFetched = onFeedFetched;

            _webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = _webRequest as HttpWebRequest;
            httpRequest.UserAgent = "PodPuppy " + Statics.VersionNumber + " (www.podpuppy.net)";

            // Annoyingly we have to this bit on a background thread
            // becuase sometimes HttpWebRequest.BeginGetResponse blocks!!!
            Thread thread = new Thread(new ThreadStart(DoFetch));
            thread.Start();

            return true;
        }

        private void DoFetch()
        {
            _webRequest.BeginGetResponse(OnGotResponseStream, null);
        }

        public void CancelFetch()
        {
            // TODO - possible cross-thread issues
            if (_webRequest != null)
                _webRequest.Abort();

            _canceled = true;
        }

        private void OnGotResponseStream(IAsyncResult result)
        {
            try
            {
                WebResponse response = _webRequest.EndGetResponse(result);

                Stream responseStream = response.GetResponseStream();

                MemoryStream memoryStream = new MemoryStream();

                byte[] buffer = new byte[ReadBufferLengthBytes];
                int offset = 0;
                int bytesRead = 0;
                do
                {
                    bytesRead = responseStream.Read(buffer, 0, ReadBufferLengthBytes);
                    memoryStream.Write(buffer, 0, bytesRead);
                    offset += bytesRead;
                    
                    if (_canceled)
                    {
                        if (_onFeedFetched != null)
                        {
                            _onFeedFetched(FeedRefreshResult.Canceled, null, null, null);
                            return;
                        }
                    }

                } while (bytesRead > 0);

                // finished with _webRequest, can re-enter now.
                _webRequest = null;

                try
                {
                    memoryStream.Position = 0;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(memoryStream);

                    if (_onFeedFetched != null)
                    {
                        _onFeedFetched(FeedRefreshResult.Success, doc, null, _url);
                        return;
                    }
                }
                catch (XmlException)
                {
                    // maybe its non-xml html with a link to a feed..
                    
                    try
                    {
                        memoryStream.Position = 0;
                        string content = new StreamReader(memoryStream).ReadToEnd();

                        MatchCollection matches = _altLinkRgx.Matches(content);
                        foreach (Match match in matches)
                        {
                            string linkContent = match.Groups[1].Value;
                            if (linkContent.Contains("type=\"application/rss+xml\""))
                            {
                                Match urlMatch = _altLinkURLRgx.Match(linkContent);
                                if (urlMatch.Success)
                                {
                                    if (_onFeedFetched != null)
                                    {
                                        string redirectUrl = urlMatch.Groups[1].Value;

                                        Fetch(redirectUrl, _onFeedFetched, false);

                                        return;
                                    }
                                }
                            }
                        }

                        if (_onFeedFetched != null)
                        {
                            _onFeedFetched(FeedRefreshResult.InvalidData, null, "Cannot connect to podcast, the resource does not appear to contain a podcast.", null);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_onFeedFetched != null)
                        {
                            _onFeedFetched(FeedRefreshResult.UnableToConnect, null, "Cannot connect to podcast, " + ex.Message, null);
                            return;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Message == "The request was aborted: The request was canceled.")
                {
                    if (_onFeedFetched != null)
                    {
                        _onFeedFetched(FeedRefreshResult.Canceled, null, null, null);
                        return;
                    }
                }
                else
                {
                    if (_onFeedFetched != null)
                    {
                        _onFeedFetched(FeedRefreshResult.UnableToConnect, null, ex.Message, null);
                        return;
                    }
                }
            }
            finally 
            { 
                _busy = false;
            }
        }

        private void OnRedirectFetched(FeedRefreshResult status, XmlDocument feed, string errorMsg, string url)
        {
            if (_onFeedFetched != null)
                _onFeedFetched(status, feed, errorMsg, url);
        }
    }
}
