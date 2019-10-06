// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;

namespace PodPuppy
{
    public class FeedFetcher
    {
        public const int ReadBufferLengthBytes = 1024;

        private bool _busy;
        private bool _canceled;
        private string _url;
        private OnFeedFetchedHandler _onFeedFetched;

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

        public void FetchIfNotBusy(string url, OnFeedFetchedHandler onFeedFetched)
        {
            Fetch(url, onFeedFetched, true);
        }

        private async void Fetch(string url, OnFeedFetchedHandler onFeedFetched, bool externalCall)
        {
            if (externalCall && _busy)
                return;
            _busy = true;

            _url = url;

            _canceled = false;
            _onFeedFetched = onFeedFetched;
                    
            var client = new System.Net.Http.HttpClient(new System.Net.Http.HttpClientHandler() { AllowAutoRedirect = true });            
            
            try
            {
                var content = await client.GetStringAsync(_url);

                if (_canceled)
                    return;

                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);

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
            catch(Exception ex) 
            {
                if (_onFeedFetched != null)
                {
                    _onFeedFetched(FeedRefreshResult.UnableToConnect, null, ex.Message, null);
                    return;
                }
            }
            finally
            {
                _busy = false;
            }           
        }

        public void CancelFetch()
        {
            _canceled = true;
        }
    }
}
