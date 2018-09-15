//////////////////////////////////////////////////////////////////////////////////
//                                                                              //
// Title: PodPuppy                                                              //
// Author: Felix Watts                                                          //
// Contact: felix@fwatts.info                                                   //
// Website: http://fwatts.info                                                  //
//                                                                              //
// This file is distributed under the Creative Commons Attribution 2.5 Licence. //
// http://creativecommons.org/licenses/by/2.5/                                  //
//                                                                              //
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Threading;
using Dolinay;

namespace PodPuppy
{
    public partial class MainForm : Form
    {
        #region Private Fields

        private Queue<string> _pendingImports;

        private static string _runFlag = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PodPuppy\\running";

        private int _minutesToNextCheck = 0;

        private FormWindowState _prevWindowState = FormWindowState.Maximized;

        private bool _syncronising = false;

        private bool _sortDirection = true;

        private BackgroundWorker _syncWorker = null;

        private BackgroundWorker _refreshWorker = null;

        private DriveDetector _driveDetector = null;

        private static TraceSwitch _traceSwitch = null;

        private FeedPropertiesDialog _subscribeDialog = null;

        private Feed _fetchingFeed = null; // the feed that is curently being fetched for the first time or null if none.

        private FeedFetchDlg _feedFetchDialog = null;

        #endregion

        #region Construction

        public MainForm(string[] args)
        {
            _pendingImports = new Queue<string>();

            InitializeComponent();
          
            _traceSwitch = new TraceSwitch("MainTrace", "");

            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(asm.Location);
            Statics.VersionNumber = info.FileVersion;

            Statics.Config = Config.Load();

            Statics.FeedListView = listFeeds;
            Statics.ItemListView = listItems;
            Statics.NotifyIcon = notifyIcon;
            Statics.DownloadManager = new DownloadManager();
            Statics.DownloadWorkerPool = new DownloadWorkerPool();
            Statics.DownloadWorkerPool.SetNumWorkers(Statics.Config.MaxDownloads);
            Statics.Scheduler = new Scheduler();
            Statics.NotifyIconManager = new NotifyIconManager();
            Statics.ThrottledStreamPool = new ThrottledStreamPool();
            Statics.ThrottledStreamPool.MaximumBytesPerSecond = Statics.Config.MaxBandwidthInBytes;
            Statics.NormalFont = new Font(Statics.ItemListView.Font.FontFamily, Statics.ItemListView.Font.Size);
            Statics.BoldFont = new Font(Statics.ItemListView.Font.FontFamily, Statics.ItemListView.Font.Size, FontStyle.Bold);

            SetUpSyncFolderWatcher();
            
            EnableDisableButtons();           

            LoadState();

            if (File.Exists(_runFlag))
            {
                // dirty shutdown.
                foreach (Feed feed in Statics.FeedListView.Items)
                    feed.RecoverFromDirtyShutdown();
            }

            if(!Directory.Exists(Path.GetDirectoryName(_runFlag)))
                Directory.CreateDirectory(Path.GetDirectoryName(_runFlag));
            StreamWriter tmp = File.CreateText(_runFlag);
            tmp.Close();
            tmp.Dispose();

            // set up sync worker
            _syncWorker = new BackgroundWorker();
            _syncWorker.WorkerReportsProgress = true;
            _syncWorker.WorkerSupportsCancellation = true;
            _syncWorker.DoWork += new DoWorkEventHandler(DoSyncronise);
            _syncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnSyncCompleted);
            _syncWorker.ProgressChanged += new ProgressChangedEventHandler(SyncProgressChanged);

            // sync scheduler timer
            Statics.Scheduler.PauseDownloads += new EventHandler(SchedulerPauseDownloads);
            Statics.Scheduler.ResumeDownloads += new EventHandler(SchedulerResumeDownloads);
            if(Statics.Config.EnableScheduler)
                Statics.Scheduler.Start();

            if (args != null && args.Length > 0)
                AddFeed(args[0]);
        }

        #endregion

        #region Overrides

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (_driveDetector != null)
            {
                _driveDetector.WndProc(ref m);
            }

            string receivedMessage = InterprocessCommunication.ProcessReceivedMessage(m);
            if (receivedMessage != null)
            {
                Activate();
                AddFeed(receivedMessage);

            }
        }

        #endregion

        #region Public Methods

        public void CancelFetch()
        {
            if(_fetchingFeed != null)
                _fetchingFeed.CancelRefresh();
        }

        #endregion

        #region Private Methods

        #region Scheduler

        private void SchedulerResumeDownloads(object sender, EventArgs e)
        {
            if (!Statics.Paused)
                return;

            Resume();
            notifyIcon.ShowBalloonTip(5000, "Downloads Resumed", "Scheduler resumed downloads", ToolTipIcon.Info);
        }

        private void SchedulerPauseDownloads(object sender, EventArgs e)
        {
            if (Statics.Paused)
                return;

            Pause();
            notifyIcon.ShowBalloonTip(5000, "Downloads Paused", "Scheduler paused downloads", ToolTipIcon.Info);
        }

        #endregion

        #region Subscribe and Search

        private void SubscribeOrSearch()
        {
            if (toolStripTextBox.Text.StartsWith("http://") || toolStripTextBox.Text.StartsWith("https://"))
            {
                AddFeed(toolStripTextBox.Text);
            }
            else Search(toolStripTextBox.Text);

            toolStripTextBox.Text = "subscribe or search";
        }

        private void Search(string keywords)
        {
            string q = Statics.Config.SearchMode == SearchMode.GoogleWithFileType ?
                "http://www.google.com/search?q=<keywords>+podcast+filetype%3Arss+OR+<keywords>+podcast+filetype%3Axml"
                : "http://www.google.com/search?q=<keywords>+podcast";
            keywords = Uri.EscapeUriString(keywords);
            q = q.Replace("<keywords>", keywords);
            try
            {
                Process.Start(q);
            }
            catch { }
        }

        #endregion

        #region Syncronisation

        private void SetUpSyncFolderWatcher()
        {
            if(Statics.Config.AutoSync && Statics.Config.SyncFolder != "")
            {
                if (_driveDetector == null)
                {
                    _driveDetector = new DriveDetector(this);
                    _driveDetector.DeviceArrived += new DriveDetectorEventHandler(OnDriveConnected);
                }
            }            
            else if (_driveDetector != null)
            {
                _driveDetector.DeviceArrived -= new DriveDetectorEventHandler(OnDriveConnected);
                _driveDetector.Dispose();
                _driveDetector = null;
            }
        }     

        private void OnDriveConnected(object sender, DriveDetectorEventArgs e)
        {
            DriveInfo dInfo = new DriveInfo(e.Drive[0].ToString());

            if ((Statics.Config.SyncVolumeLabel == "" || 
                Statics.Config.SyncVolumeLabel == dInfo.VolumeLabel) && 
                Statics.Config.AutoSync && e.Drive[0] == Statics.Config.SyncFolder[0])
            {
                Syncronise();
            }
        }

        private void Syncronise()
        {
            if (_syncronising)
                return;

            if (_syncWorker.IsBusy)
                return;

            // no sync folder
            if (Statics.Config.SyncFolder == "")
                return;

            // drive not connected
            if (!Directory.Exists(Path.GetPathRoot(Statics.Config.SyncFolder)))
                return;

            _syncronising = true;
            EnableDisableButtons();
            toolStripStatusLabel1.Text = "Syncronising...";
            syncProgressBar.Visible = true;
            lnkCancelSync.Visible = true;

            List<string> syncedFeeds = new List<string>();
            foreach (Feed feed in Statics.FeedListView.Items)
                if (feed.Syncronised)
                    syncedFeeds.Add(Statics.Config.GetCompleteDestination(feed));

            _syncWorker.RunWorkerAsync(syncedFeeds);

        }

        private void SyncProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Syncronising: " + (string)e.UserState + "...";
            syncProgressBar.Value = e.ProgressPercentage;
        }

        private void OnSyncCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Cancelled ? "Syncronisation canceled." : "Syncronisation complete.";
            syncProgressBar.Visible = false;
            lnkCancelSync.Visible = false;

            _syncronising = false;
            EnableDisableButtons();
        }
       
        private void DoSyncronise(object sender, DoWorkEventArgs e)
        {
            if (!Directory.Exists(Statics.Config.SyncFolder))
            {
                try
                {
                    Directory.CreateDirectory(Statics.Config.SyncFolder);
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Unable to create the Syncronisation Folder. Make sure you have write access to " + Statics.Config.SyncFolder + " or select a different Syncronisation Folder.", "Create Syncronisation Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Trace.WriteLine("Sycronisation Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("PodPuppy encountered an unexpected problem during syncronisation: " + ex.Message, "Syncronise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Trace.WriteLine("Sycronisation Error: " + ex.Message);
                }                

            }

            BackgroundWorker worker = _syncWorker;// (BackgroundWorker)e.Argument;
            List<string> syncedFeeds = (List<string>)e.Argument;

            try
            {
                //SyncFolders(Statics.Config.CompletedFilesBaseDirectory, Statics.Config.SyncFolder);

                List<string> srcFiles = new List<string>();

                string[] extensions = Statics.Config.SyncedFileTypes.Split(' ');

                foreach (string feedDir in syncedFeeds)
                {
                    foreach(string ext in extensions)
                        srcFiles.AddRange(Directory.GetFiles(feedDir, "*." + ext, SearchOption.TopDirectoryOnly));
                }

                int srcLength = Statics.Config.CompletedFilesBaseDirectory.Length;

                // get dest list now before it gets any bigger
                string[] destFiles = Directory.GetFiles(Statics.Config.SyncFolder, "*.*", SearchOption.AllDirectories);

                string[] destFolders = Directory.GetDirectories(Statics.Config.SyncFolder, "*", SearchOption.AllDirectories);

                float numFiles = (float)(srcFiles.Count + destFiles.Length + destFolders.Length + 1);
                float processedFiles = 0f;

                int destLength = Statics.Config.SyncFolder.Length;

                // 
                // 1) Delete files that exist on device but not in synced feeds
                // 

                foreach (string destFile in destFiles)
                {
                    if (worker.CancellationPending)
                        return;

                    string relDest = destFile.Substring(destLength);
                    string srcFile = Statics.Config.CompletedFilesBaseDirectory + relDest;
                    if (!srcFiles.Contains(srcFile))//File.Exists(srcFile))
                    {
                        int progress = (int)((processedFiles / numFiles) * 100f);
                        worker.ReportProgress(progress, "Removing " + Path.GetFileName(destFile));

                        File.Delete(destFile);
                    }

                    processedFiles++;
                }

                //
                // 2) Remove empty directories from device
                //

                foreach (string dir in Directory.GetDirectories(Statics.Config.SyncFolder))
                {
                    if (worker.CancellationPending)
                        return;

                    if (Directory.GetDirectories(dir).Length == 0
                        && Directory.GetFiles(dir).Length == 0)
                    {
                        int progress = (int)((processedFiles / numFiles) * 100f);
                        worker.ReportProgress(progress, "Removing " + dir);

                        Directory.Delete(dir);
                    }

                    processedFiles++;
                }

                //
                // 3) copy files to device
                //

                foreach (string srcFile in srcFiles)
                {
                    if (worker.CancellationPending)
                        return;

                    string relSrc = srcFile.Substring(srcLength);
                    string destFile = Statics.Config.SyncFolder + relSrc;

                    string destDir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);

                    if (!File.Exists(destFile))
                    {
                        int progress = (int)((processedFiles / numFiles) * 100f);
                        worker.ReportProgress(progress, "Adding " + Path.GetFileName(destFile));

                        File.Copy(srcFile, destFile);
                    }
                    else
                    {
                        // if the dest file exists, check it hasnt changed
                        FileInfo srcInfo = new FileInfo(srcFile);
                        FileInfo destInfo = new FileInfo(destFile);
                        if (srcInfo.LastWriteTime > destInfo.LastWriteTime)
                        {
                            int progress = (int)((processedFiles / numFiles) * 100f);
                            worker.ReportProgress(progress, "Adding " + Path.GetFileName(destFile));

                            File.Copy(srcFile, destFile, true);
                        }
                    }

                    processedFiles++;                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("PodPuppy encountered a problem during syncronisation: " + ex.Message, "Syncronise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Trace.WriteLine("Sycronisation Error: " + ex.Message);
            }
        }

        //private void DeleteEmptyDirectories(string root, BackgroundWorker worker, float numFiles, ref float proccessedDirectories, ref string lastDeletedDirectory)
        //{
        //    string[] subDirs = Directory.GetDirectories(root);
        //    foreach (string subDir in subDirs)
        //        DeleteEmptyDirectories(subDir, worker, numFiles, ref proccessedDirectories, ref lastDeletedDirectory);

        //    string[] files = Directory.GetFiles(root);
        //    subDirs = Directory.GetDirectories(root);
        //    if (root != Statics.Config.SyncFolder && files.Length == 0 && subDirs.Length == 0)
        //    {
        //        Directory.Delete(root);
        //        lastDeletedDirectory = root;
        //    }

        //    proccessedDirectories++;
        //    int progress = (int)((proccessedDirectories / numFiles) * 100f);
        //    worker.ReportProgress(progress, "Removed " + lastDeletedDirectory);
        //}

        #endregion        

        #region Version Checking

        private void CheckForNewerVersion()
        {
            if (!Statics.Config.CheckForNewVersion)
                return;

            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(DoCheckForNewerVersion);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnCheckForNewerVersionCompleted);
                worker.RunWorkerAsync(worker);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Version check error: " + ex.Message);
            }

            //try
            //{
            //    string url = System.Configuration.ConfigurationManager.AppSettings["versionCheckURL"];
            //    if (url == null)
            //        url = "http://fwatts.info/podpuppy/version.txt";
            //    WebRequest request = WebRequest.Create(url);
            //    request.BeginGetResponse(new AsyncCallback(OnCheckNewerVersionComplete), request);
            //}
            //catch (Exception ex)
            //{
            //    Trace.TraceError("Version check error: " + ex.Message);
            //}
        }

        private void OnCheckForNewerVersionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Statics.ItemListView.InvokeRequired)
            {
                Statics.ItemListView.Invoke(new RunWorkerCompletedEventHandler(OnCheckForNewerVersionCompleted), sender, e);
                return;
            }

            if (e.Result == null)
                return;

            string msg = e.Result as string;

            try
            {
                if (MessageBox.Show(this, msg, "PodPuppy Version Check", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    string url = System.Configuration.ConfigurationManager.AppSettings["downloadsPage"];
                    if (url == null)
                        url = "http://fwatts.info/podpuppy/downloads.html";
                    System.Diagnostics.Process.Start(url);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Version check error: " + ex.Message);
            }
        }

        void DoCheckForNewerVersion(object sender, DoWorkEventArgs e)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["versionCheckURL"];
                if (url == null)
                    url = "http://fwatts.info/podpuppy/version.txt";
                WebRequest request = WebRequest.Create(url);

                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string newestVersion = reader.ReadLine();
                string msg = reader.ReadToEnd();

                if (CompareVersionStrings(Statics.VersionNumber, newestVersion) >= 0)
                {
                    e.Result = null;
                    return;
                }

                string msg2 = "A newer version of PodPuppy exists. Would you like to go to the downloads page?";
                if (msg != null && msg != "")
                    msg2 = msg2 + "\n\n" + msg;

                e.Result = msg2;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Version check error: " + ex.Message);
                e.Result = null;
            }
        }

        //private void OnCheckNewerVersionComplete(IAsyncResult result)
        //{
        //    if (Statics.ItemListView.InvokeRequired)
        //    {
        //        Statics.ItemListView.Invoke(new AsyncCallback(OnCheckNewerVersionComplete), result);
        //        return;
        //    }

        //    try
        //    {
        //        WebRequest request = (WebRequest)result.AsyncState;
        //        WebResponse response = request.EndGetResponse(result);
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        string newestVersion = reader.ReadLine();
        //        string msg = reader.ReadToEnd();

        //        if (CompareVersionStrings(Statics.VersionNumber, newestVersion) >= 0)
        //            return;

        //        string msg2 = "A newer version of PodPuppy exists. Would you like to go to the downloads page?";
        //        if (msg != null && msg != "")
        //            msg2 = msg2 + "\n\n" + msg;

        //        if (MessageBox.Show(this, msg2, "PodPuppy Version Check", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            string url = System.Configuration.ConfigurationManager.AppSettings["downloadsPage"];
        //            if (url == null)
        //                url = "http://fwatts.info/podpuppy/downloads.html";
        //            System.Diagnostics.Process.Start(url);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.TraceError("Version check error: " + ex.Message);
        //    }
        //}

        private int CompareVersionStrings(string v1, string v2)
        {
            string[] v1Arr = v1.Split('.');
            string[] v2Arr = v2.Split('.');

            for (int n = 0; n < 4; n++)
            {
                int v1Num = int.Parse(v1Arr[n]);
                int v2Num = int.Parse(v2Arr[n]);

                if (v1Num > v2Num)
                    return 1;
                else if (v1Num < v2Num)
                    return -1;
            }

            return 0;
        }

        #endregion

        #region Feed Lifecycle        

        private void RefreshFeeds()
        {
            // if there is no dyniamic OPML then just refresh the current feeds
            if (Statics.Config.DynamicOPMLSource == "")
            {
                foreach (Feed feed in Statics.FeedListView.Items)
                    feed.Refresh();
            }
            // otherwise we need to update subscriptions before refreshing feeds.
            else
            {
                if (_refreshWorker == null)
                {
                    _refreshWorker = new BackgroundWorker();
                    _refreshWorker.DoWork += new DoWorkEventHandler(_refreshWorker_DoWork);
                    _refreshWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_refreshWorker_RunWorkerCompleted);
                }
                if (_refreshWorker.IsBusy)
                    return;

                _refreshWorker.RunWorkerAsync();
            }
        }

        private void _refreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<string> opmlSubs = (List<string>)e.Result;
            List<string> currentSubs = new List<string>();
            foreach (Feed feed in Statics.FeedListView.Items)
            {
                if (feed.AddedDynamically && !opmlSubs.Contains(feed.URL))
                    feed.Remove(Statics.Config.DynamicOPMLDeleteFiles);
                else currentSubs.Add(feed.URL);
            }
            foreach (string url in opmlSubs)
            {
                if (!currentSubs.Contains(url))
                    AddFeed(url, true);
            }
            
            // finished upfating dyniamic subscriptions, now refresh feeds.
            foreach (Feed feed in Statics.FeedListView.Items)
                feed.Refresh();
        }

        private void _refreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebRequest request = WebRequest.Create(Statics.Config.DynamicOPMLSource);
                WebResponse response = request.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                List<string> opmlSubs = new List<string>();
                foreach (XmlNode xmlUrl in doc.SelectNodes("//outline/@xmlUrl"))
                {
                    string url = xmlUrl.Value;
                    opmlSubs.Add(url);
                }

                e.Result = opmlSubs;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error fetching Dynamic OPML from URL {0} : {1}", Statics.Config.DynamicOPMLSource, ex.Message);

                e.Result = new List<string>();
            }
        }

        private void CheckSelectedFeeds()
        {
            foreach (Feed feed in Statics.FeedListView.SelectedItems)
                feed.Refresh();
        }

        private void RemoveSelectedFeeds()
        {
            foreach (Feed feed in Statics.FeedListView.SelectedItems)
            {
                int priority = feed.Priority;

                bool deleteFiles = false;
                if (Directory.Exists(Statics.Config.GetCompleteDestination(feed)))
                {
                    
                    switch (new UnsubscribeDialog(Statics.Config.GetCompleteDestination(feed)).ShowDialog(this))
                    {
                        case DialogResult.Yes:
                            deleteFiles = false;
                            break;
                        case DialogResult.No:
                            deleteFiles = true;
                            break;
                        case DialogResult.Cancel:
                            return;
                    }                    
                }

                feed.Remove(deleteFiles);
                
                foreach (Feed other in Statics.FeedListView.Items)
                    if (other.Priority > priority)
                        other.Priority--;

                feed.ItemDownloaded -= OnItemDownloaded;
            }
        }

        private void RemoveDynamicSubscriptions()
        {
            foreach (Feed feed in Statics.FeedListView.Items)
                if (feed.AddedDynamically)
                    feed.Remove(Statics.Config.DynamicOPMLDeleteFiles);
        }

        private void AddFeed(string url)
        {
            AddFeed(url, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dynamically">true if the system is adding this feed due to
        /// dynamic OPML subscription. If true, user input will not be required and error
        /// messages will go to the Trace.</param>
        private void AddFeed(string url, bool dynamically)
        {
            bool error = false;

            if (!dynamically && _fetchingFeed != null)
            {
                MessageBox.Show("A feed is already being fetched. Please wait for that operation to complete.", "Add Feed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                error = true;
            }

            if (url == "" || url == "<Enter Podcast URL>")
            {
                if (dynamically)
                    Trace.TraceWarning("Dynamic OPML: Cannot subscribe to URL: {0}", url);
                else
                    MessageBox.Show("First enter the URL of the Podcast in the textbox on the toolbar.", "Add Feed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                error = true;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                if (dynamically)
                    Trace.TraceWarning("Dynamic OPML: Cannot subscribe to URL: {0}", url);
                else
                    MessageBox.Show("Unable to subscribe to podcast; invalid URL: " + url, "Add Feed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                error = true;
            }

            if (url.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase)
                || url.EndsWith(".mpg", StringComparison.CurrentCultureIgnoreCase))
            {
                // probably not an XML feed (probably an item draged from the items list view
                // and accidentally dropped in the feeds view
                error = true;
            }

            // Check this feed isnt aready added
            foreach (Feed feed in Statics.FeedListView.Items)
            {
                if (feed.URL == url)
                {
                    if (dynamically)
                        Trace.TraceWarning("Dynamic OPML: Cannot subscribe to URL: {0}. Feed already subscribed.", url);
                    else
                        MessageBox.Show("You have already subscribed to the podcast at " + feed.URL, "Add Feed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    error = true;
                }
            }

            if (error)
            {
                _fetchingFeed = null;
                ProcessNextPendingImport();                

                return;
            }

            Feed newFeed = new Feed(url);
            newFeed.ItemDownloaded += OnItemDownloaded;
            newFeed.AddedDynamically = dynamically;
            newFeed.FirstRefreshCompleted += OnFeedFetched;            

            if (dynamically)
            {
                _fetchingFeed = null;
            }
            else
            {
                _fetchingFeed = newFeed;                
                if (_feedFetchDialog == null)
                    _feedFetchDialog = new FeedFetchDlg(this);
                _feedFetchDialog.Show();
            }
                       
            newFeed.Refresh();
        }

        private void OnFeedFetched(Feed subject, FeedRefreshResult status)
        {
            if(!subject.AddedDynamically)
                _feedFetchDialog.Hide();

            switch (status)
            {
                case FeedRefreshResult.Canceled:
                case FeedRefreshResult.InvalidData:
                case FeedRefreshResult.UnableToConnect:
                default:
                   
                    subject.FirstRefreshCompleted -= OnFeedFetched;
                    // throw it away

                    break;

                case FeedRefreshResult.Success:

                    if (subject.AddedDynamically)
                    {
                        subject.Priority = Statics.FeedListView.Items.Count;
                        Statics.FeedListView.Items.Add(subject);
                        subject.RefreshStatusString();
                        Statics.DownloadManager.StartNextDownload();
                    }
                    else
                    {
                        if (_subscribeDialog == null)
                            _subscribeDialog = new SubscribeDialog2();

                        _subscribeDialog.Populate(subject);

                        if (_subscribeDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            _subscribeDialog.Apply(subject);
                            subject.Priority = Statics.FeedListView.Items.Count;
                            Statics.FeedListView.Items.Add(subject);
                            subject.RefreshStatusString();
                            Statics.DownloadManager.StartNextDownload();
                        }                        
                    }

                    break;

                case FeedRefreshResult.IsOPML:

                    if (MessageBox.Show(this, 
                        "The url contains OPML (rather than a podcast.) Would you like to set your dynamic OPML source to this url?", 
                        "Subscribe", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        Statics.Config.DynamicOPMLSource = subject.URL;
                        RefreshFeeds();
                    }
                    break;
            }

            _fetchingFeed = null;

            ProcessNextPendingImport();
        }

        private void OnItemDownloaded(Item item)
        {
            Statics.LastItemDownloaded = item;

            if (item.Feed.Selected && Statics.Config.ItemViewSortColumn == 3)
                SortItems();

            if (Statics.Config.FeedViewSortColumn == 4)
                SortFeeds();

            if (Statics.Config.ShowBalloons)
                notifyIcon.ShowBalloonTip(5, "Item Downloaded", "PodPuppy has finished downloading " + item.Title, ToolTipIcon.Info);
        }

        #endregion

        #region Options

        /// <summary>
        /// Displays the options dialog that allows users to adjust the configuration options.
        /// Then applies the new settings if they clicked OK.
        /// </summary>
        private void ShowOptionsDialog()
        {
            OptionsDlg dlg = new OptionsDlg();

            string prevDynamicOPMLSrc = Statics.Config.DynamicOPMLSource;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {                
                if (Statics.Config.DynamicOPMLSource != prevDynamicOPMLSrc)
                {
                    // dynamic src has changed - remove all dynamic subs.
                    RemoveDynamicSubscriptions();

                    if (Statics.Config.DynamicOPMLSource != "")
                        RefreshFeeds();
                }

                StartStopAutoChecking();

                if (Statics.Config.ShowInTaskbarWhenMinimized)
                    ShowInTaskbar = true;

                EnableDisableButtons();
                SetUpSyncFolderWatcher();

                SortFeeds();
                SortItems();
            }
        }

        private void StartStopAutoChecking()
        {
            if (Statics.Config.CheckFeedInterval > 0)
            {
                updateTimerTick(null, null);
                updateTimer.Start();
            }
            else
            {
                updateTimer.Stop();
                toolStripStatusLabel1.Text = "Automatic checking is turned off.";
            }
        }

        #endregion

        #region Pause / Resume

        private void Pause()
        {
            if (Statics.Paused)
                return;

            Statics.Paused = true;
            Statics.DownloadWorkerPool.StopAll();

            foreach (Feed feed in Statics.FeedListView.Items)
                feed.RefreshStatusString();

            foreach (Item item in Statics.ItemListView.Items)
                item.RefreshStatusString();

            EnableDisableButtons();

            Statics.NotifyIconManager.RefreshMode();
        }

        private void Resume()
        {
            if (!Statics.Paused)
                return;

            Statics.Paused = false;
            Statics.DownloadManager.StartAllDownloads();

            foreach (Feed feed in Statics.FeedListView.Items)
                feed.RefreshStatusString();

            foreach (Item item in Statics.ItemListView.Items)
                item.RefreshStatusString();

            EnableDisableButtons();

            Statics.NotifyIconManager.RefreshMode();
        }

        #endregion

        #region Load / Save State


        // PodPuppy aims to startup in the same state as it was when it was last shut down.
        // (it would be annoying if you had to re-subscribe to your feeds each time you started the app!)
        // The methods in this region are used to serialise the current state to disk when 
        // PodPuppy closes and re-instate it when the app starts.
        //
        // The "state" includes the details of each subscribed feed and item.

        /// <summary>
        /// Writes the current state to an XML file
        /// </summary>
        /// <returns>True on success.</returns>
        private bool SaveState()
        {
            XmlTextWriter writer = null;

            bool userRetry = false;
            string filename = Path.Combine(Statics.Config.SettingsDir, "PodPuppy.state");
            do
            {
                int retries = 3;
                while (retries > 0)
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filename));
                        Stream fileStream = File.Create(filename);
                        writer = new XmlTextWriter(fileStream, Encoding.UTF8);
                        writer.Formatting = Formatting.Indented;

                        writer.WriteStartDocument();
                        writer.WriteStartElement("PodPuppyState");

                        writer.WriteStartElement("feeds");

                        Feed[] feedArr = new Feed[listFeeds.Items.Count];
                        listFeeds.Items.CopyTo(feedArr, 0);
                        XmlSerializer feedSer = new XmlSerializer(typeof(Feed[]));
                        feedSer.Serialize(writer, feedArr);

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        return true;
                    }
                    catch (IOException ex)
                    {
                        System.Diagnostics.Trace.TraceError("****** Error saving state: " + ex.Message);
                        System.Diagnostics.Trace.TraceError("****** Retry " + retries);
                        Thread.Sleep(250);
                        retries--;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("****** Error saving state: " + ex.Message);
                        return false;
                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Flush();
                            writer.Close();
                        }
                    }
                }

                switch (MessageBox.Show(this, string.Format("PodPuppy is unable to save its state because another application is locking the following file:\n\n{0}\n\nIf you are running antivirus software, make sure that it is allowing PodPuppy to write to that file. Click Retry to retry the save, click Abort to save a backup copy of the PodPuppy state to another location. Click Ignore to close without saving.", filename), "Save State", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning))
                {
                    case DialogResult.Abort:
                        SaveFileDialog dlg = new SaveFileDialog();
                        dlg.Filter = "PodPuppy State File|PodPuppy.state";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            userRetry = true;
                            filename = dlg.FileName;
                        }
                        break;
                    case DialogResult.Retry:
                        userRetry = true;
                        break;
                    case DialogResult.Ignore:
                    default:
                        break;
                }

            } while (userRetry);

            return false;
        }

        /// <summary>
        /// Attempts to load the application state from an XML file
        /// </summary>
        /// <returns>True on success.</returns>
        private bool LoadState()
        {
            Stream fileStream = null;
            bool userRetry = false;
            string filename = Path.Combine(Statics.Config.SettingsDir, "PodPuppy.state");
            do
            {
                int retries = 3;
                while (retries > 0)
                {
                    try
                    {
                        string stateFile = Statics.Config.SettingsDir + "\\PodPuppy.state";
                        if (!File.Exists(stateFile))
                            // no state file exists. Nothing to do.
                            return true;

                        fileStream = File.OpenRead(stateFile);
                        XmlTextReader reader = new XmlTextReader(fileStream, new NameTable());
                        reader.WhitespaceHandling = WhitespaceHandling.Significant;

                        reader.ReadStartElement("PodPuppyState");
                        reader.ReadStartElement("feeds");

                        XmlSerializer ser = new XmlSerializer(typeof(Feed[]));
                        Feed[] feeds = (Feed[])ser.Deserialize(reader);

                        // can remove after version 0.6.0.0 (migration from old style state file)
                        SetFeedPrioritiesWhereMissing(feeds);

                        reader.ReadEndElement();
                        reader.ReadEndElement();

                        foreach (Feed feed in feeds)
                            feed.ItemDownloaded += OnItemDownloaded;

                        listFeeds.BeginUpdate();
                        listFeeds.Items.AddRange(feeds);
                        SortFeeds();
                        listFeeds.EndUpdate();

                        return true;
                    }
                    catch (IOException ex)
                    {
                        System.Diagnostics.Trace.TraceError("****** Error loading state: " + ex.Message);
                        System.Diagnostics.Trace.TraceError("****** Retry " + retries);
                        Thread.Sleep(250);
                        retries--;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Error loading state: " + ex.Message);

                        listFeeds.Items.Clear();

                        return false;
                    }
                    finally
                    {
                        if (fileStream != null)
                        {
                            fileStream.Close();
                        }
                    }
                }

                switch (MessageBox.Show(this, string.Format("PodPuppy is unable to load its previous state because another application is locking the following file:\n\n{0}\n\nIf you are running antivirus software, make sure that it is allowing PodPuppy to read that file. Click Retry to retry the load, click Cancel to continue without loading state.", filename), "Load State", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Retry:
                        userRetry = true;
                        break;
                    case DialogResult.Cancel:
                    default:
                        break;
                }

            } while (userRetry);

            return false;
        }

        // used for migrating from old system where feeds had no priorities
        private void SetFeedPrioritiesWhereMissing(Feed[] feeds)
        {
            int maxPrioritySoFar = -1;
            foreach (Feed feed in feeds)
            {
                if (feed.Priority == -1)
                {
                    maxPrioritySoFar++;
                    feed.Priority = maxPrioritySoFar;
                }
                else if (feed.Priority > maxPrioritySoFar)
                    maxPrioritySoFar = feed.Priority;
            }
        }

        #endregion

        #region Import / Export Subscriptions

        private void ImportOPML()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Subscription Lists (*.opml)|*.opml|All Files (*.*)|*.*";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(dlg.FileName);

                    foreach (XmlNode xmlUrl in doc.SelectNodes("/opml/body/outline/@xmlUrl"))
                    {
                        _pendingImports.Enqueue(xmlUrl.Value);
                        //string url = xmlUrl.Value;
                        //AddFeed(url, false);
                    }

                    ProcessNextPendingImport();
                }
                catch (Exception ex)
                {                    
                    System.Diagnostics.Trace.TraceError("Error importing OPML: " + ex.Message);
                    MessageBox.Show("Sorry, the selected file is not in a format supported by PodPuppy.", "Import Subscriptions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ProcessNextPendingImport()
        {            
            if(_pendingImports.Count == 0)
                return;

            string url = _pendingImports.Dequeue();

            AddFeed(url, false);
        }

        private void ExportOPML()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Subscription Lists (*.opml)|*.opml";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.FileName = "PodPuppy Subscriptions.opml";

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                XmlWriter writer = null;

                try
                {
                    writer = new XmlTextWriter(dlg.FileName, Encoding.UTF8);

                    writer.WriteStartDocument();
                    writer.WriteStartElement("opml");
                    writer.WriteAttributeString("version", "1.1");

                    writer.WriteStartElement("head");
                    writer.WriteElementString("title", "Popuppy Subscriptions");
                    writer.WriteElementString("dateCreated", DateTime.Now.ToString());
                    writer.WriteElementString("dateModified", DateTime.Now.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("body");
                    foreach (Feed feed in Statics.FeedListView.Items)
                    {
                        writer.WriteStartElement("outline");
                        writer.WriteAttributeString("text", feed.Title);
                        writer.WriteAttributeString("xmlUrl", feed.URL);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to import subscriptions: " + ex.Message, "Import Subscriptions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }

        #endregion

        #region Housekeeping

        private void EnableDisableButtons()
        {
            int numSelectedFeeds = Statics.FeedListView.SelectedItems.Count;
            int numSelectedItems = Statics.ItemListView.SelectedItems.Count;

            btnPauseDownloads.Enabled = !Statics.Paused;
            btnResumeDownloads.Enabled = Statics.Paused;
            menuMainPodcastsResume.Enabled = Statics.Paused;
            menuMainPodcastsPause.Enabled = !Statics.Paused;
            menuIconPause.Enabled = !Statics.Paused;
            menuIconResume.Enabled = Statics.Paused;

            menuFeedRefresh.Enabled = numSelectedFeeds > 0;
            menuFeedRemove.Enabled = numSelectedFeeds > 0;
            btnRemoveSelectedFeeds.Enabled = numSelectedFeeds > 0;

            Feed firstSelectedFeed = null;
            if (numSelectedFeeds >= 1)
                firstSelectedFeed = Statics.FeedListView.SelectedItems[0] as Feed;

            btnRaisePriority.Enabled = (numSelectedFeeds == 1) && (firstSelectedFeed.Priority > 0);
            btnLowerPriority.Enabled = (numSelectedFeeds == 1) && (firstSelectedFeed.Priority < Statics.FeedListView.Items.Count - 1);
            menuFeedCopyUrl.Enabled = numSelectedFeeds == 1;

            Item firstSelectedItem = null;
            if(numSelectedItems >= 1)
                firstSelectedItem = Statics.ItemListView.SelectedItems[0] as Item;

            bool canPlayItem = numSelectedItems == 1
                && firstSelectedItem.Status == ItemStatus.Complete;
            menuItemPlay.Enabled = canPlayItem;

            bool canOpenItemFolder = canPlayItem;
            menuItemOpenFolder.Enabled = canOpenItemFolder;

            bool canTryAgain = numSelectedItems == 1
                && firstSelectedItem.Status == ItemStatus.Error;
            menuItemTryAgain.Visible = canTryAgain;

            bool canCopyItemURL = numSelectedItems == 1;
            menuItemCopyUrl.Enabled = canCopyItemURL;

            bool canViewItemInfo = numSelectedItems == 1
                && firstSelectedItem.Description != String.Empty;
            menuItemViewInfo.Enabled = canViewItemInfo;

            bool skipSelectedEnabled = false;            
            foreach (Item item in Statics.ItemListView.SelectedItems)
                if (item.Status != ItemStatus.Skip)
                {
                    skipSelectedEnabled = true;
                    break;
                }
            menuItemSkipSelected.Enabled = skipSelectedEnabled;

            bool unskipSelectedEnabled = false;
            foreach (Item item in Statics.ItemListView.SelectedItems)
                if (item.Status == ItemStatus.Skip)
                {
                    unskipSelectedEnabled = true;
                    break;
                }
            menuItemUnskipSelected.Enabled = unskipSelectedEnabled;

            menuItemTags.Enabled = numSelectedItems == 1
                && firstSelectedItem.Status == ItemStatus.Complete
                && firstSelectedItem.CompleteDestination.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase);

            //menuFeedOpenFolder.Enabled = (numSelectedFeeds == 1);
            //menuFeedCopyUrl.Enabled = (numSelectedFeeds == 1);
            //menuFeedPlay.Enabled = (numSelectedFeeds == 1);
            //menuFeedOldItems.Enabled = (numSelectedFeeds == 1);

            bool allSynced = true;
            foreach(Feed feed in Statics.FeedListView.SelectedItems)
            {
                if(!feed.Syncronised)
                    allSynced = false;
            }
            //menuFeedSyncronise.Checked = allSynced;

            //if (numSelectedFeeds == 1)
            //{
            //    Feed selectedFeed = Statics.FeedListView.SelectedItems[0] as Feed;

            //    foreach (ToolStripMenuItem menuItem in menuFeedOldItems.DropDownItems)
            //        menuItem.Checked = selectedFeed.ArchiveMode == (ArchiveMode)menuItem.Tag;
            //}            

            menuMainFileSyncronise.Enabled = !_syncronising && Statics.Config.SyncFolder != "" &&
                Directory.Exists(Path.GetPathRoot(Statics.Config.SyncFolder));
        }

        private void SortFeeds()
        {
            Comparison<Feed> comparer = null;
            switch (Statics.Config.FeedViewSortColumn)
            {
                case 0:
                    // order by title
                    comparer = new Comparison<Feed>(Feed.NameComparer);
                    break;
                case 1:
                    // order by status
                    comparer = new Comparison<Feed>(Feed.StatusComparer);
                    break;
                case 2:
                    // order by whether syncronized
                    comparer = new Comparison<Feed>(Feed.SyncronisedComparer);
                    break;
                case 3:
                    // order by whether syncronized
                    comparer = new Comparison<Feed>(Feed.PriorityComparer);
                    break;
                case 4:
                    // order by time of last download
                    comparer = new Comparison<Feed>(Feed.LatestItemComparer);
                    break;
            }

            Feed[] feeds = new Feed[Statics.FeedListView.Items.Count];
            Statics.FeedListView.Items.CopyTo(feeds, 0);
            Array.Sort<Feed>(feeds, comparer);

            if (!Statics.Config.FeedSortAscending)
                Array.Reverse(feeds);

            Statics.FeedListView.BeginUpdate();
            Statics.FeedListView.Items.Clear();
            Statics.FeedListView.Items.AddRange(feeds);
            Statics.FeedListView.EndUpdate();
        }

        private void SortItems()
        {
            Comparison<Item> comparer = null;
            switch (Statics.Config.ItemViewSortColumn)
            {
                case 0:
                    // order by title
                    comparer = new Comparison<Item>(Item.NameComparer);
                    break;
                case 1:
                    // order by status
                    comparer = new Comparison<Item>(Item.StatusComparer);
                    break;
                case 2:
                    // order by pub date
                    comparer = new Comparison<Item>(Item.PublicationDateComparer);
                    break;
                case 3:
                    // order by dl date
                    comparer = new Comparison<Item>(Item.DownloadDateComparer);
                    break;
            }

            Item[] items = new Item[Statics.ItemListView.Items.Count];
            Statics.ItemListView.Items.CopyTo(items, 0);
            Array.Sort<Item>(items, comparer);

            if (!Statics.Config.ItemSortAscending)
                Array.Reverse(items);

            Statics.ItemListView.BeginUpdate();
            Statics.ItemListView.Items.Clear();
            Statics.ItemListView.Items.AddRange(items);
            Statics.ItemListView.EndUpdate();
        }

        //private void TruncateTraceFile()
        //{
        //    if(Trace.Listeners.Count == 0)
        //        return;

        //    if(!Trace.Listeners[0] is TextWriterTraceListener)
        //        return;

        //    string filename = Application.StartupPath + "\\" + Trace.Listeners[0].Attributes["initializeData"];
        //    if(!File.Exists(filename))
        //        return;

        //    string trStr = System.Configuration.ConfigurationManager.AppSettings["traceMaxLength"];
        //    int maxLength = 1048576;
        //    if (trStr != null)
        //        int.TryParse(trStr, out maxLength);

        //    FileInfo fi = new FileInfo(filename);
        //    if (fi.Length > maxLength)
        //    {
        //        Trace.Listeners[0].
        //    }
        //}

        #endregion

        #region Event Handlers

        #region Feed Related

        //private void menuFeedOldItemsSubItemClick(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
        //    if (menuItem == null)
        //        return;

        //    if (menuItem.Checked)
        //        return;

        //    Feed selectedFeed = Statics.FeedListView.SelectedItems.Count == 1 ? Statics.FeedListView.SelectedItems[0] as Feed : null;
        //    if (selectedFeed == null)
        //        return;

        //    selectedFeed.ArchiveMode = (ArchiveMode)menuItem.Tag;

        //    foreach (ToolStripMenuItem item2 in menuItem.Owner.Items)
        //        item2.Checked = item2 == menuItem;

        //    selectedFeed.Refresh();
        //}

        private void RemoveSelectedFeedsClick(object sender, EventArgs e)
        {
            RemoveSelectedFeeds();
        }

        private void btnRaisePriorityClick(object sender, EventArgs e)
        {
            if (Statics.FeedListView.SelectedItems.Count != 1)
                return;

            Feed feed = (Feed)Statics.FeedListView.SelectedItems[0];
            if (feed.Priority == 0)
                return;


            Feed swap = null;
            foreach(Feed other in Statics.FeedListView.Items)
                if (other.Priority == feed.Priority - 1)
                {
                    swap = other;
                    break;
                }

            swap.Priority = feed.Priority;
            feed.Priority--;

            if (swap.Status == FeedStatus.Downloading)
                swap.GetDownloadingItem().StopDownload();

            if (Statics.Config.FeedViewSortColumn == 3) // priority column
            {
                SortFeeds();
                Statics.FeedListView.EnsureVisible(feed.Priority);
            }
        }

        private void btnLowerPriorityClick(object sender, EventArgs e)
        {
            if (Statics.FeedListView.SelectedItems.Count != 1)
                return;

            Feed feed = (Feed)Statics.FeedListView.SelectedItems[0];

            Feed swap = null;
            foreach (Feed other in Statics.FeedListView.Items)
                if (other.Priority == feed.Priority + 1)
                {
                    swap = other;
                    break;
                }

            if (swap == null)
                return; // already lowest priority

            swap.Priority = feed.Priority;
            feed.Priority++;

            if (swap.Status == FeedStatus.Downloading)
                swap.GetDownloadingItem().StopDownload();

            if (Statics.Config.FeedViewSortColumn == 3) // priority column
            {
                SortFeeds();
                Statics.FeedListView.EnsureVisible(feed.Priority);
            }
        }

        private void menuFeedOpening(object sender, CancelEventArgs e)
        {
            EnableDisableButtons();
        }

        private void menuFeedRefreshClick(object sender, EventArgs e)
        {
            CheckSelectedFeeds();
        }

        private void menuFeedOpenFolderClick(object sender, EventArgs e)
        {
            foreach (Feed feed in Statics.FeedListView.SelectedItems)
            {
                feed.OpenContainingFolder();
                return;
            }
        }

        private void removeToolStripMenuIte_Click(object sender, EventArgs e)
        {
            RemoveSelectedFeeds();
        }

        private void menuFeedCopyUrlClick(object sender, EventArgs e)
        {
            foreach (Feed feed in listFeeds.SelectedItems)
            {
                Clipboard.SetText(feed.URL);
                return;
            }
        }

        private void menuFeedPlayClick(object sender, EventArgs e)
        {
            foreach (Feed feed in Statics.FeedListView.SelectedItems)
            {
                feed.Play();
                return;
            }
        }

        #endregion

        #region Item Related

        private void menuItemViewInfoClick(object sender, EventArgs e)
        {
            ViewItemInfo();
        }

        private void menuItemTryAgainClick(object sender, EventArgs e)
        {
            foreach (Item item in Statics.ItemListView.SelectedItems)
                item.TryAgain();
        }

        private void menuItemPlayClick(object sender, EventArgs e)
        {
            foreach (Item item in Statics.ItemListView.SelectedItems)
            {
                item.Play();
                return;
            }
        }

        private void menuItemOpening(object sender, CancelEventArgs e)
        {
            EnableDisableButtons();
        }

        private void menuItemOpenFolderClick(object sender, EventArgs e)
        {
            foreach (Item item in Statics.ItemListView.SelectedItems)
            {
                item.OpenContainingFolder();
                return;
            }
        }

        private void menuItemCopyUrlClick(object sender, EventArgs e)
        {
            foreach (Item item in listItems.SelectedItems)
            {
                Clipboard.SetText(item.URL);
                return;
            }
        }

        private void menuItemUnskipSelectedClick(object sender, EventArgs e)
        {
            SkipSelectedItems(false);
        }

        private void menuItemSkipSelectedClick(object sender, EventArgs e)
        {
            SkipSelectedItems(true);
        }

        #endregion

        #region Global

        private void PauseClick(object sender, EventArgs e)
        {
            Pause();
        }

        private void ResumeClick(object sender, EventArgs e)
        {
            Resume();
        }

        private void btnAddClick(object sender, EventArgs e)
        {
            SubscribeOrSearch();
        }

        private void menuMainHelpUserGuideClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Application.StartupPath + "\\Help\\PodPuppy User Guide.html");
            }
            catch { }
        }

        private void OpenPodcastsFolderClick(object sender, EventArgs e)
        {
            OpenPodcastsFolder();
        }

        private void toolStripTextBoxEnter(object sender, EventArgs e)
        {
            toolStripTextBox.Text = "";
        }

        private void toolStripTextBoxTextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = toolStripTextBox.Text != "" && toolStripTextBox.Text != "subscribe or search";
        }

        private void toolStripTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SubscribeOrSearch();
            }
        }

        private void RefreshAllClick(object sender, EventArgs e)
        {
            updateTimer.Stop();
            _minutesToNextCheck = 0;
            updateTimerTick(null, null);
            if(Statics.Config.CheckFeedInterval > 0)
                updateTimer.Start();
        }

        private void menuMainHelpAboutClick(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        private void menuMainHelpVisitWebsiteClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/felixwatts/PodPuppy");
            }
            catch (Exception) { }
        }

        private void reportABugToolStripMenuIte_Click(object sender, EventArgs e)
        {
            MailErrorReport();
        }

        private void menuMainFileOptionsClick(object sender, EventArgs e)
        {
            ShowOptionsDialog();
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void menuMainFileImportClick(object sender, EventArgs e)
        {
            ImportOPML();
        }

        private void menuMainFileSyncroniseClick(object sender, EventArgs e)
        {
            Syncronise();
        }

        private void menuMainFileExportCLick(object sender, EventArgs e)
        {
            ExportOPML();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            if (_syncWorker != null && _syncWorker.IsBusy)
                _syncWorker.CancelAsync();
        }

        #endregion

        #region Feeds List View

        private void listFeedsColumnClick(object sender, ColumnClickEventArgs e)
        {
            if(Statics.Config.FeedViewSortColumn == e.Column)
                Statics.Config.FeedSortAscending = !Statics.Config.FeedSortAscending;
            else Statics.Config.FeedViewSortColumn = e.Column;

            SortFeeds();
        }  

        private void listFeedsSelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();

            if (listFeeds.SelectedItems.Count == 1)
            {
                (listFeeds.SelectedItems[0] as Feed).PopulateItemsListView();
                SortItems();
            }
            else
            {
                Statics.ItemListView.Items.Clear();
            }
        }

        private void listFeedsDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("UniformResourceLocator"))
                e.Effect = DragDropEffects.None;
            else if ((e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
                e.Effect = DragDropEffects.Link;
            else if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                e.Effect = DragDropEffects.Copy;
        }

        private void listFeedsDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string url = e.Data.GetData("UniformResourceLocator") as string;
                if (url == null)
                {
                    Stream urlStream = e.Data.GetData("UniformResourceLocator") as Stream;
                    StreamReader reader = new StreamReader(urlStream, Encoding.ASCII);
                    url = reader.ReadToEnd();
                    url = url.Replace("\0", "");
                }

                if (url != null)
                    AddFeed(url);
            }
            catch (Exception) { }
        }

        private void listFeedsItemDrag(object sender, ItemDragEventArgs e)
        {
            Feed feed = e.Item as Feed;

            DataObject data = new DataObject();
            data.SetData("UniformResourceLocator", feed.URL);
            data.SetData("FileNameW", feed.PlaylistFilename);
            List<string> itemFiles = new List<string>();
            foreach (Item item in feed.Items)
                if (item.Status == ItemStatus.Complete)
                    itemFiles.Add(item.CompleteDestination);
            data.SetData(DataFormats.FileDrop, itemFiles.ToArray());

            DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Link);
        }

        private void listFeedsItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.Item.Selected)
                (e.Item as Feed).Highlighted = false;
        }

        #endregion

        #region Items List View

        private void listItemsItemDrag(object sender, ItemDragEventArgs e)
        {
            Item item = e.Item as Item;

            DataObject data = new DataObject();
            data.SetData("UniformResourceLocator", item.URL);
            if (item.Status == ItemStatus.Complete)
                data.SetData("FileNameW", item.CompleteDestination);

            List<string> selectedItems = new List<string>();
            foreach (Item selectedItem in Statics.ItemListView.SelectedItems)
                if (selectedItem.Status == ItemStatus.Complete)
                    selectedItems.Add(selectedItem.CompleteDestination);
            data.SetData(DataFormats.FileDrop, selectedItems.ToArray());

            DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Link);
        }

        private void listItemsColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == Statics.Config.ItemViewSortColumn)
                Statics.Config.ItemSortAscending = !Statics.Config.ItemSortAscending;
            else Statics.Config.ItemViewSortColumn = e.Column;

            SortItems();
        }        

        private void listItemsItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.Item.Selected)
                (e.Item as Item).Highlighted = false;
        }

        private void listItemsDoubleClick(object sender, EventArgs e)
        {
            foreach (Item item in Statics.ItemListView.SelectedItems)
            {
                item.Play();
                break;
            }
        }

        #endregion

        #region Main Form

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (_syncWorker.IsBusy)
                    _syncWorker.CancelAsync();

                // stops downloads from starting when workers become available
                Statics.Paused = true;
                toolStripStatusLabel1.Text = "Shutting down...";
                Statics.DownloadWorkerPool.Dispose(); // will block until all downloads are stopped
                SaveState();
                SaveFormState();                                               

                // mark clean shut down.
                File.Delete(_runFlag);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Shutdown error: " + ex.Message);
            }
            finally
            {
                Trace.Flush();
                Trace.Close();
            }
        }

        private void SaveFormState()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Statics.Config.MainFormWidth = Size.Width;
                Statics.Config.MainFormHeight = Size.Height;

                Statics.Config.MainFormLeft = Location.X;
                Statics.Config.MainFormTop = Location.Y;
            }
            else
            {
                Statics.Config.MainFormWidth = RestoreBounds.Size.Width;
                Statics.Config.MainFormHeight = RestoreBounds.Size.Height;

                Statics.Config.MainFormLeft = RestoreBounds.Location.X;
                Statics.Config.MainFormTop = RestoreBounds.Location.Y;
            }

            Statics.Config.MainSplitterLocation = listFeeds.Size.Height;

            Statics.Config.Save();
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            if (!this.Created)
                return;

            if (Statics.Config.ShowInTaskbarWhenMinimized)
                return;

            if (WindowState != _prevWindowState)
            {
                _prevWindowState = WindowState;

                switch (WindowState)
                {
                    case FormWindowState.Minimized:
                        if (ShowInTaskbar)
                        {
                            ShowInTaskbar = false;
                            FormBorderStyle = FormBorderStyle.FixedToolWindow; // stop it from appearing in task switcher (alt-tab)
                        }
                        break;
                    default:
                        if (!ShowInTaskbar)
                        {
                            ShowInTaskbar = true;
                            FormBorderStyle = FormBorderStyle.Sizable;
                        }
                        break;
                }
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            if (Statics.Config.StartMinimised)
                WindowState = FormWindowState.Minimized;
            // correctly sets the ShowInTaskBar state.
            else MainFormResize(null, null);

            this.Size = new Size(Statics.Config.MainFormWidth, Statics.Config.MainFormHeight);
            this.Location = new Point(Statics.Config.MainFormLeft, Statics.Config.MainFormTop);
            this.listFeeds.Size = new Size(listFeeds.Size.Width, Statics.Config.MainSplitterLocation);

            CheckForNewerVersion();

            StartStopAutoChecking();
        }

        #endregion

        #region NotifyIcon

        private void notifyIconBalloonTipClicked(object sender, EventArgs e)
        {
            switch (Statics.Config.BalloonFunction)
            {
                case BalloonFunction.OpenFolder:
                    if (Statics.LastItemDownloaded != null)
                        Statics.LastItemDownloaded.OpenContainingFolder();
                    break;

                case BalloonFunction.PlayFile:
                    if (Statics.LastItemDownloaded != null)
                        Statics.LastItemDownloaded.Play();
                    break;
            }
        }

        private void notifyIconDoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            Activate();
        }

        #endregion

        #region Refresh Timer

        private void updateTimerTick(object sender, EventArgs e)
        {
            _minutesToNextCheck--;
            if (_minutesToNextCheck <= 0)
            {
                _minutesToNextCheck = Statics.Config.CheckFeedInterval;
                RefreshFeeds();
            }

            if (!_syncronising)
            {
                if (Statics.Config.CheckFeedInterval == 0)
                    toolStripStatusLabel1.Text = "Automatic checking is turned off.";
                else if(_minutesToNextCheck > 60)
                {
                    int mins = _minutesToNextCheck % 60;
                    int hours = (int)Math.Floor((double)_minutesToNextCheck / 60d);
                    toolStripStatusLabel1.Text = "Checking feeds in " + hours + ":" + mins.ToString("00");
                }
                else 
                    toolStripStatusLabel1.Text = "Checking feeds in " + _minutesToNextCheck + " minutes.";
            }
                
        }

        #endregion                          

        #endregion                

        #region Other

        private void OpenPodcastsFolder()
        {
            try
            {
                Process.Start(Statics.Config.CompletedFilesBaseDirectory);
            }
            catch (Exception) { }
        }

        private void ViewItemInfo()
        {
            foreach (Item item in Statics.ItemListView.SelectedItems)
            {
                new HTMLViewDlg(item).ShowDialog(this);
                break;
            }
        }

        private void SkipSelectedItems(bool skip)
        {
            if (Statics.ItemListView.SelectedItems.Count == 0)
                return;

            Statics.DeleteCompletedItemsOnSkip = false;
            if (skip)
            {
                foreach (Item item in Statics.ItemListView.SelectedItems)
                {
                    if (item.Status == ItemStatus.Complete)
                    {
                        switch(new DeleteFileDlg().ShowDialog(this))
                        {
                            case DialogResult.Cancel:
                                return;
                            case DialogResult.Yes:
                                Statics.DeleteCompletedItemsOnSkip = true;
                                break;
                            case DialogResult.No:
                                Statics.DeleteCompletedItemsOnSkip = false;
                                break;
                        }
                        break;
                    }
                }
            }

            foreach (Item item in Statics.ItemListView.SelectedItems)
            {
                item.Skip(skip);
            }
        }

        private void SimpleMode(bool yes)
        {
            if (yes)
            {
                Size = MinimumSize;
                listItems.Visible = false;
            }
            else
            {
                listItems.Visible = true;
                Size = new Size(488, 451);
            }
        }

        private void MailErrorReport()
        {
            new ReportBugDlg().ShowDialog(this);
        }

        #endregion                        
       
        #endregion       

        //private void menuFeedTags_Click(object sender, EventArgs e)
        //{
        //    if(Statics.FeedListView.SelectedItems.Count == 0)
        //        return;

        //    Feed feed = (Feed)Statics.FeedListView.SelectedItems[0];

        //    new FeedTagsDlg(feed).ShowDialog(this);
        //}

        private void menuItemTags_Click(object sender, EventArgs e)
        {
            if (Statics.ItemListView.SelectedItems.Count == 0)
                return;

            Item item = (Item)Statics.ItemListView.SelectedItems[0];

            new ItemTagsDlg(item).ShowDialog(this);
        }

        private void listFeeds_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedFeeds();
            }
        }

        private void OnMenuFeedPropertiesClick(object sender, EventArgs e)
        {
            if(Statics.FeedListView.SelectedItems.Count == 0)
                return;

            Feed feed = (Feed)Statics.FeedListView.SelectedItems[0];

            FeedPropertiesDialog dlg = new FeedPropertiesDialog();
            dlg.Populate(feed);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                dlg.Apply(feed);
                feed.Refresh();
            }
        }
    }
}