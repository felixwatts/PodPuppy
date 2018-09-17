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
using System.Drawing;
using System.IO;

namespace PodPuppy
{
    /// <summary>
    /// This class contains members that are made static because we want them to be available
    /// everywhere.
    /// </summary>
    public class Statics
    {
        /// <summary>
        /// The application version number.
        /// </summary>
        private static string _versionNumber;
        public static string VersionNumber
        {
            get { return _versionNumber; }
            set { _versionNumber = value; }
        }

        /// <summary>
        /// Holds the current app configuration settings.
        /// </summary>
        private static Config _config;

        /// <summary>
        /// Accesses the current app configuration settings.
        /// </summary>
        public static Config Config
        {
            get { return _config; }
            set { _config = value; }
        }

        private static DownloadManager _downloadManager;
        public static DownloadManager DownloadManager
        {
            get { return _downloadManager; }
            set { _downloadManager = value; }
        }

        private static DownloadWorkerPool _downloadWorkerPool;
        public static DownloadWorkerPool DownloadWorkerPool
        {
            get { return _downloadWorkerPool; }
            set { _downloadWorkerPool = value; }
        }

        private static ListView _feedListView;
        public static ListView FeedListView
        {
            get { return _feedListView; }
            set { _feedListView = value; }
        }

        private static ListView _itemListView;
        public static ListView ItemListView
        {
            get { return _itemListView; }
            set { _itemListView = value; }
        }

        private static NotifyIcon _notifyIcon;
        public static NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set { _notifyIcon = value; }
        }

        private static Item _lastItemDownloaded;
        public static Item LastItemDownloaded
        {
            get { return _lastItemDownloaded; }
            set { _lastItemDownloaded = value; }
        }

        private static bool _paused = false;
        public static bool Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }

        private static bool _closing = false;
        public static bool Closing
        {
            get { return _closing; }
            set { _closing = value; }
        }

        private static bool _enableItemCheckBoxes = true;
        public static bool EnableItemCheckBoxes
        {
            get { return _enableItemCheckBoxes; }
            set { _enableItemCheckBoxes = value; }
        }

        private static Scheduler _scheduler;
        public static Scheduler Scheduler
        {
            get { return _scheduler; }
            set { _scheduler = value; }
        }

        private static NotifyIconManager _notifyIconManger;
        public static NotifyIconManager NotifyIconManager
        {
            get { return _notifyIconManger; }
            set { _notifyIconManger = value; }
        }

        private static Font _normalFont = null;
        public static Font NormalFont
        {
            get { return _normalFont; }
            set { _normalFont = value; }
        }

        private static Font _boldFont = null;
        public static Font BoldFont
        {
            get { return _boldFont; }
            set { _boldFont = value; }
        }

        private static FileSystemWatcher _syncFolderWatcher = null;
        public static FileSystemWatcher SyncFolderWatcher
        {
            get { return _syncFolderWatcher; }
            set { _syncFolderWatcher = value; }
        }

        private static bool _deleteCompletedItemsOnSkip = false;
        public static bool DeleteCompletedItemsOnSkip
        {
            get { return _deleteCompletedItemsOnSkip; }
            set { _deleteCompletedItemsOnSkip = value; }
        }

        private static ThrottledStreamPool _throttledStreamPool = null;
        public static ThrottledStreamPool ThrottledStreamPool
        {
            get { return _throttledStreamPool; }
            set { _throttledStreamPool = value; }
        }
    }
}
