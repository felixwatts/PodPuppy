using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace PodPuppy
{

    /// <summary>
    /// Represents a pool of background workers for downloading items. The number of workers can be set and the pool can be asked to download
    /// an item.
    /// </summary>
    public class DownloadWorkerPool : IDisposable
    {
        #region Enumerations

        public enum State
        {
            Available,
            Unavalable
        }

        #endregion

        #region Private Members

        private List<BackgroundWorker> _workers;
        private List<BackgroundWorker> _removedWorkers;
        private List<bool> _busyWorkerMask; // records which workers a re currentyl busy OR reserved but not yet started.
        private Dictionary<Item, BackgroundWorker> _itemToWorkerLookup;
        private Dictionary<BackgroundWorker, Item> _workerToItemLookup;
        private bool _disposing = false;
        private State _state;

        #endregion

        #region Construction

        public DownloadWorkerPool()
        {
            _itemToWorkerLookup = new Dictionary<Item, BackgroundWorker>();
            _workerToItemLookup = new Dictionary<BackgroundWorker, Item>();
            _workers = new List<BackgroundWorker>();
            _removedWorkers = new List<BackgroundWorker>();
            _busyWorkerMask = new List<bool>();
            _state = State.Available;
        }

        #endregion

        #region Public Methods

        public void SetNumWorkers(int numWorkers)
        {
            // We dont want to allow other threads to start or stop downloads during this process
            // as the worker pool may not be in a consistent state
            _state = State.Unavalable;

            int currentNum = _workers.Count;
            if (numWorkers > currentNum)
            {
                for (int i = currentNum; i < numWorkers; i++)
                    AddWorker();
            }
            else if (numWorkers < currentNum)
            {
                int numToRemove = currentNum - numWorkers;

                // first remove workers that aren't busy
                while (numToRemove > 0)
                {
                    int i = _busyWorkerMask.IndexOf(false);
                    if (i >= 0)
                        RemoveWorker(_workers[i]);
                    else
                    {
                        // all workers in use.
                        // remove the worker that is downloading the feed with the lowest priority of all downloading feeds

                        for (int rank = Statics.FeedListView.Items.Count - 1; rank >= 0; rank--)
                        {
                            Feed feed = Statics.FeedListView.Items[rank] as Feed;
                            if (feed.Status == FeedStatus.Downloading)
                            {
                                Item downloadingItem = feed.GetDownloadingItem();

                                if (downloadingItem == null)
                                    continue;

                                BackgroundWorker worker = _itemToWorkerLookup[downloadingItem];

                                RemoveWorker(worker);

                                break;
                            }
                        }
                    }

                    numToRemove--;
                }
            }

            _state = State.Available;
        }

        public bool HasAvailableWorker
        {
            get
            {
                return _state == State.Available && GetAvailableWorker() != null;
            }
        }

        public bool Download(Item item)
        {
            lock (this)
            {

                if (_disposing)
                    return false;

                if (item.Status != ItemStatus.Downloading)
                    return false;

                if (item.Feed.Status != FeedStatus.Downloading)
                    return false;

                while (_state != State.Available) { } // wait until worker pool has finished adding or removing workers

                BackgroundWorker worker = GetAvailableWorker();
                if (worker == null)
                    return false;

                // reserve worker so it cant be re-allocated. Note: we cant use worker.IsBusy because
                // in Feed.ChangeStatus below we may try to reallocate the same worker
                // which is not yet marked as busy.
                _busyWorkerMask[_workers.IndexOf(worker)] = true;

                _workerToItemLookup.Add(worker, item);
                _itemToWorkerLookup.Add(item, worker);

                worker.RunWorkerAsync(item);

                return true;
            }
        }

        public void StopAll()
        {
            foreach (Item item in _itemToWorkerLookup.Keys)
            {
                item.StopDownload();
            }
        }

        #endregion

        #region Events and Delegates

        public delegate void DownloadWorkerPoolEventHandler(Item item);

        public delegate void DownloadProgressChangedHandler(Item item, int percent);

        /// <summary>
        /// Fired when an item has finished downloading
        /// </summary>
        public event DownloadProgressChangedHandler ItemProgressChanged;

        //public event DownloadWorkerPoolEventHandler WorkerFinished;

        #endregion

        #region Private Methods

        // this is the slow method that actually downloads a file
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == null)
                throw new ArgumentException("downloadWorker_DoWork: sender must be a BackgroundWorker");
            Item item = e.Argument as Item;
            if (item == null)
                throw new ArgumentException("downloadWorker_DoWork: async argument must be an Item.");

            Stream responseStream = null;
            Stream fileStream = null;
            try
            {

                WebRequest request = null;
                WebResponse response = null;
                do
                {
                    request = WebRequest.Create(item.URL);
                    HttpWebRequest httpRequest = request as HttpWebRequest;
                    httpRequest.UserAgent = "PodPuppy " + Statics.VersionNumber + " github.com/felixwatts/PodPuppy";

                    if (httpRequest == null)
                        throw new NotSupportedException("Cannot download item '" + item.Title + "', Only HTTP resources are currently supported.");

                    if (item.BytesDownloaded != 0)
                    {
                        // find out if webserver supports range requests
                        //
                        // easiest way to do this is just to try a range request

                        httpRequest.AddRange("bytes", -(int)(item.ContentLengthBytes - item.BytesDownloaded));
                    }

                    try
                    {
                        response = request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response == null)
                        {       
                            // Workaround. For some reason if ex.Response is null it still tries to evaluate
                            // the latter parts of the if statement below!!!

                            // some kind of server error, unable to download item
                            item.Error("Cannot download item: " + ex.Message);
                            e.Result = item;
                            return;
                        }
                        if (ex.Response != null && (ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.MethodNotAllowed || (ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotAcceptable)
                        {
                            // range request not supported, restart from beginning
                            item.BytesDownloaded = 0;
                            request = null;

                            if (File.Exists(item.DownloadDestination))
                                File.Delete(item.DownloadDestination);
                        }
                        else
                        {
                            // some kind of server error, unable to download item
                            item.Error("Cannot download item: " + ex.Message);
                            e.Result = item;
                            return;
                        }
                    }

                } while (response == null);


                HttpWebResponse httpResponse = response as HttpWebResponse;
                if (httpResponse == null)
                    throw new NotSupportedException("Cannot download item '" + item.Title + "', Only HTTP resources are currently supported.");

                if (!Directory.Exists(Path.GetDirectoryName(item.DownloadDestination)))
                    Directory.CreateDirectory(Path.GetDirectoryName(item.DownloadDestination));
                
                // did server support byte range?
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.PartialContent:

                        fileStream = File.Open(item.DownloadDestination, FileMode.Append);

                        ContentRange range =
                            ContentRange.Parse(httpResponse.Headers[HttpResponseHeader.ContentRange]);
                        if (range == null)
                            throw new WebException("Invalid content range header: " + httpResponse.Headers[HttpResponseHeader.ContentRange]);

                        if (range.RangeMin != item.BytesDownloaded)
                            throw new Exception("Received wrong byte range from http server.");

                        item.ContentLengthBytes = range.Total;                        

                        break;

                    case HttpStatusCode.OK:

                        // some servers seem to return OK when they dont support range requests
                        // and instead restart from the beginning.
                        fileStream = File.Open(item.DownloadDestination, FileMode.Create);
                        item.BytesDownloaded = 0;

                        item.ContentLengthBytes = httpResponse.ContentLength;

                        break;

                    case HttpStatusCode.NotFound:

                        throw new Exception("Item not found on server.");

                    default:
                        throw new Exception("Unexpected HTTP response code when downloading item: "
                            + httpResponse.StatusDescription);
                }

                responseStream = response.GetResponseStream();
                if (Statics.Config.MaxBandwidthInBytes > 0)
                    responseStream = Statics.ThrottledStreamPool.AddStream(responseStream);

                int bytesRead = -1; // not 0 because thats used after the loop to see if worker was cancelled.
                byte[] buffer = new byte[1024];

                try
                {
                    do
                    {
                        if (item.Status != ItemStatus.Downloading)
                            break;

                        bytesRead = responseStream.Read(buffer, 0, 1024);
                        fileStream.Write(buffer, 0, bytesRead);
                        item.BytesDownloaded += bytesRead;

                    } while (bytesRead != 0);
                }
                catch (Exception ex)
                {
                    item.Error("Cannot download item: " + ex.Message);
                }
                finally
                {
                    fileStream.Flush();
                    fileStream.Close();                 
                    fileStream = null;
                    responseStream.Close();
                    responseStream = null;
                }

                if (bytesRead == 0)
                {
                    // download completed succesfully

                    string moveToDir = item.CompleteDestination;
                    if (!Directory.Exists(Path.GetDirectoryName(moveToDir)))
                        Directory.CreateDirectory(Path.GetDirectoryName(moveToDir));
                    if (File.Exists(moveToDir))
                        File.Delete(moveToDir);
                    File.Move(item.DownloadDestination, moveToDir);

                    item.SetDownloadComplete();
                }
                else if (item.Feed.Status == FeedStatus.Removed || item.Status == ItemStatus.Skip || item.Status == ItemStatus.Error)
                {
                    // delete partial file because something went wrong
                    File.Delete(item.DownloadDestination);
                }
                
                // Now we've moved/deleted the download cache file, delete its directory if its now empty.
                string downloadDir = Path.GetDirectoryName(item.DownloadDestination);
                if (Directory.GetFiles(downloadDir).Length == 0)
                    Directory.Delete(downloadDir);                
            }
            catch (ArgumentNullException)
            {
                item.Error("Cannot download item '" + item.Title + "', URL is null");
            }
            catch (System.Security.SecurityException)
            {
                item.Error("Cannot download item '" + item.Title + "', we do not have permission to access the resource.");
            }
            catch (NotSupportedException)
            {
                item.Error("Cannot download item '" + item.Title + "', the protocol is not suppoorted.");
            }
            catch (IOException ex)
            {
                item.Error("Cannot download item '" + item.Title + "', " + ex.Message);
            }
            catch (Exception ex)
            {
                item.Error("Cannot download item '" + item.Title + "', an unforseen problem occurred: " + ex.Message);
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();
                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            e.Result = item;
        }

        // WARNING: always check that _state is Available before calling this.
        private BackgroundWorker GetAvailableWorker()
        {
            foreach (BackgroundWorker worker in _workers)
            {
                if (!WorkerIsBusy(worker))
                    return worker;
            }
            return null;
        }

        // WARNING: always set _state to unavailable before running this.
        private void AddWorker()
        {
            BackgroundWorker newWorker = new BackgroundWorker();
            newWorker.WorkerReportsProgress = true;
            newWorker.WorkerSupportsCancellation = true;
            newWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            newWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            newWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _busyWorkerMask.Add(false);
            _workers.Add(newWorker);
            OnWorkerFree();

        }

        // WARNING: always set _state to unavailable before running this.
        private void RemoveWorker(BackgroundWorker worker)
        {
            if (!_workers.Contains(worker) || _removedWorkers.Contains(worker))
                return;

            if (WorkerIsBusy(worker))
            {
                Item item = _workerToItemLookup[worker];
                item.StopDownload();
                //item.Status = ItemStatus.WorkerRemoved;
                //item.CancelDownload();
                _removedWorkers.Add(worker);
                //worker.CancelAsync();

            }
            else
            {
                // worker not busy - remove it now
                _busyWorkerMask.RemoveAt(_workers.IndexOf(worker));
                _workers.Remove(worker);
            }
        }

        private bool WorkerIsBusy(BackgroundWorker worker)
        {
            if (!_workers.Contains(worker))
                return true;

            return _busyWorkerMask[_workers.IndexOf(worker)];
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ItemProgressChanged != null)
                ItemProgressChanged(e.UserState as Item, e.ProgressPercentage);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Item item = e.Result as Item;
            BackgroundWorker worker = sender as BackgroundWorker;

            lock (this)
            {
                while (worker.IsBusy) { } // shouldnt need this but it seems that the item file is still open when we try to write the  tags 

                _workerToItemLookup.Remove(worker);
                _itemToWorkerLookup.Remove(item);

                if (_removedWorkers.Contains(worker))
                {
                    _busyWorkerMask.RemoveAt(_workers.IndexOf(worker));
                    _workers.Remove(worker);
                    _removedWorkers.Remove(worker);
                }
                else _busyWorkerMask[_workers.IndexOf(worker)] = false;
            }

            item.OnDownloadWorkerFinished();

            OnWorkerFree();
        }

        private void OnWorkerFree()
        {
            Statics.DownloadManager.StartNextDownload();
        }

        //private int CalculatePercentComplete(Item item)
        //{
        //    if (item.ContentLength == 0)
        //        item.PercentComplete = 100;
        //    else item.PercentComplete = (int)((item.BytesDownloaded * 100) / item.ContentLength);
        //    return item.PercentComplete;
        //}

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _disposing = true;

            //foreach (BackgroundWorker worker in _workers)
            //    worker.CancelAsync();
            StopAll();

            // TODO wait for all workers to complete?
            bool allFinished = false;
            while (!allFinished)
            {
                Thread.Sleep(500);
                Application.DoEvents();
                allFinished = true;
                foreach (BackgroundWorker worker in _workers)
                {
                    if (worker.IsBusy)
                    {
                        allFinished = false;
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
