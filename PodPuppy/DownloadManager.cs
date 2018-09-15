using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public class DownloadManager
    {
        public DownloadManager()
        {
        }

        public delegate bool StartNextDownloadHandler();

        public bool StartNextDownload()
        {
            if (Statics.FeedListView.InvokeRequired)
                return (bool)Statics.FeedListView.Invoke(new StartNextDownloadHandler(StartNextDownload));

            if (Statics.Paused || Statics.Closing)
                return false;

            if(!Statics.DownloadWorkerPool.HasAvailableWorker)
                return false;

            List<Feed> feeds = new List<Feed>();
            foreach (Feed feed in Statics.FeedListView.Items)
                feeds.Add(feed);
            feeds.Sort(new Comparison<Feed>(Feed.PriorityComparer));

            foreach(Feed feed in feeds)
            {
                if (feed.StartNextDownload())
                    return true;
            }

            return false;
        }

        public void StartAllDownloads()
        {
            while (StartNextDownload()) { }
        }
    }
}
