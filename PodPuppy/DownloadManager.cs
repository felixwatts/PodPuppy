// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

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
