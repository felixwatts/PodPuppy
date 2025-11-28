// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PodPuppy
{
    public partial class OptionsDlg : CentredDialog
    {

        private int _maxDownloads;
        public int MaxDownloads
        {
            get { return _maxDownloads; }
            set 
            { 
                _maxDownloads = value;
                txtMaxDownloads.Text = value.ToString();
            }
        }

        private int _checkInterval;
        public int CheckInterval
        {
            get { return _checkInterval; }
            set
            {
                _checkInterval = value;
                txtCheckInterval.Text = value.ToString();
            }
        }

        private long _maxBandwidhInBytes;
        private long MaxBandwidthInBytes
        {
            get { return _maxBandwidhInBytes; }
            set
            {
                _maxBandwidhInBytes = value;
                txtLimitBandwidth.Text = ((long)(value / 1024)).ToString();
            }
        }
	
        public OptionsDlg()
        {            
            InitializeComponent();           
            Populate();
        }     

        private void btnBrowseTempFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDownloadFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtDownloadFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnBrowseCompleteFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtCompleteFolder.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                txtCompleteFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void Populate()
        {
            CheckInterval = Statics.Config.CheckFeedInterval > 60 ? Statics.Config.CheckFeedInterval / 60 : Statics.Config.CheckFeedInterval;
            cmbCheckIntervalUnits.SelectedItem = Statics.Config.CheckFeedInterval > 60 ? "Hours" : "Minutes";
            MaxDownloads = Statics.Config.MaxDownloads;
            txtDownloadFolder.Text = Statics.Config.DownloadBaseDirectory;
            txtCompleteFolder.Text = Statics.Config.CompletedFilesBaseDirectory;
            chkLaunchOnStartup.Checked = Statics.Config.LaunchOnStartup;
            chkShowBalloons.Checked = Statics.Config.ShowBalloons;
            rbBalloonPlayFile.Checked = Statics.Config.BalloonFunction == BalloonFunction.PlayFile;
            chkStartMinimised.Checked = Statics.Config.StartMinimised;
            chkShowInTaskbarWhenMinimized.Checked = Statics.Config.ShowInTaskbarWhenMinimized;
            chkIncludeFiletypeInSearch.Checked = Statics.Config.SearchMode == SearchMode.GoogleWithFileType;
            chkWritePlaylists.Checked = Statics.Config.WritePlaylists;

            txtSyncFolder.Text = Statics.Config.SyncFolder;
            chkAutoSync.Checked = Statics.Config.AutoSync;
            _syncVolumeLabel = Statics.Config.SyncVolumeLabel;

            MaxBandwidthInBytes = Statics.Config.MaxBandwidthInBytes;

            txtSyncedFileTypes.Text = Statics.Config.SyncedFileTypes;

            txtDynamicSubscriptionSrc.Text = Statics.Config.DynamicOPMLSource;
            chkDynamicOPMLJustGetLatest.Checked = Statics.Config.DynamicOPMLJustGetLatest;
            chkDynamicOPMLDeleteFiles.Checked = Statics.Config.DynamicOPMLDeleteFiles;

            scheduleControl.Data = (bool[,])Statics.Config.Schedule.Clone();
            chkEnableScheduler.Checked = Statics.Config.EnableScheduler;
            scheduleControl.Enabled = Statics.Config.EnableScheduler;

        }        

        private void PopulateShedulerTab()
        {                            
        }

        private void Apply()
        {
            Statics.Config.CheckFeedInterval = ((string)cmbCheckIntervalUnits.SelectedItem) == "Hours" ? CheckInterval * 60 : CheckInterval;
            Statics.Config.MaxDownloads = MaxDownloads;
            Statics.Config.CompletedFilesBaseDirectory = txtCompleteFolder.Text;
            Statics.Config.DownloadBaseDirectory = txtDownloadFolder.Text;         
            Statics.Config.LaunchOnStartup = chkLaunchOnStartup.Checked;
            Statics.Config.ShowBalloons = chkShowBalloons.Checked;
            Statics.Config.BalloonFunction = rbBalloonPlayFile.Checked ? BalloonFunction.PlayFile : BalloonFunction.OpenFolder;
            Statics.Config.StartMinimised = chkStartMinimised.Checked;
            Statics.Config.ShowInTaskbarWhenMinimized = chkShowInTaskbarWhenMinimized.Checked;
            Statics.Config.SearchMode = chkIncludeFiletypeInSearch.Checked ? SearchMode.GoogleWithFileType : SearchMode.Google;
            Statics.Config.WritePlaylists = chkWritePlaylists.Checked;

            Statics.Config.SyncFolder = txtSyncFolder.Text;
            Statics.Config.AutoSync = chkAutoSync.Checked;
            Statics.Config.SyncVolumeLabel = _syncVolumeLabel;

            Statics.Config.MaxBandwidthInBytes = (int)MaxBandwidthInBytes;
            Statics.ThrottledStreamPool.MaximumBytesPerSecond = Statics.Config.MaxBandwidthInBytes;

            Statics.Config.SyncedFileTypes = txtSyncedFileTypes.Text.ToLower();

            Statics.Config.DynamicOPMLSource = txtDynamicSubscriptionSrc.Text;
            Statics.Config.DynamicOPMLJustGetLatest = chkDynamicOPMLJustGetLatest.Checked;
            Statics.Config.DynamicOPMLDeleteFiles = chkDynamicOPMLDeleteFiles.Checked;

            Statics.Config.Schedule = (bool[,])scheduleControl.Data.Clone();
            Statics.Config.EnableScheduler = chkEnableScheduler.Checked;

            Statics.Config.Save();

            if (Statics.Config.EnableScheduler)
                Statics.Scheduler.Start();
            else Statics.Scheduler.Stop();

            Statics.DownloadWorkerPool.SetNumWorkers(Statics.Config.MaxDownloads);

            Statics.DownloadManager.StartAllDownloads();            
        }

        private void txtMaxDownloads_Validating(object sender, CancelEventArgs e)
        {
            // max downloads
            if (!int.TryParse(txtMaxDownloads.Text, out _maxDownloads) 
                || _maxDownloads < 0 || _maxDownloads > 20)
            {
                MessageBox.Show("Maximum Downloads must be a number between 1 and 20.", "Validate Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
             
        }

        private void chkShowBalloons_CheckedChanged(object sender, EventArgs e)
        {
            gbBalloonFunction.Enabled = chkShowBalloons.Checked;
        }

        private void OptionsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                Apply();
        }

        private void chkEnableScheduler_CheckedChanged(object sender, EventArgs e)
        {
            scheduleControl.Enabled = chkEnableScheduler.Checked;
        }

        private void txtCheckInterval_Validating(object sender, CancelEventArgs e)
        {
            // check interval
            if (!int.TryParse(txtCheckInterval.Text, out _checkInterval)
                || _checkInterval < 0)
            {
                MessageBox.Show("Feed Check Interval must be a positive integer.", "Validate Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }            
        }

        private string _syncVolumeLabel;

        private void btnBrowseSyncFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = txtSyncFolder.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSyncFolder.Text = dlg.SelectedPath;
                DriveInfo dInfo = new DriveInfo(txtSyncFolder.Text.Substring(0, 1));
                _syncVolumeLabel = dInfo.VolumeLabel;
            }
        }

        private void OptionsDlg_Load(object sender, EventArgs e)
        {
            // centre on main form
            int left = Owner.Left + ((Owner.Width - this.Width) / 2);
            int top = Owner.Top + ((Owner.Height - this.Height) / 2);
            this.Location = new Point(left, top);
        }

        private void txtLimitBandwidth_Validating(object sender, CancelEventArgs e)
        {
            long maxBandwidthInKilobytes;
            if (!long.TryParse(txtLimitBandwidth.Text, out maxBandwidthInKilobytes)
                || maxBandwidthInKilobytes < 0)
            {
                MessageBox.Show("Bandwidth limit must be a positive integer or 0.", "Validate Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else _maxBandwidhInBytes = maxBandwidthInKilobytes * 1024;
        }

        private void btnRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, 
                "Do you really want to restore default configuration? All your configuration settings will be lost.", "Restore Defaults", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Statics.Config = new Config();
                Populate();
            }
        }

        private void txtDownloadFolder_Validating(object sender, CancelEventArgs e)
        {
            if (!Tools.IsValidPath(txtDownloadFolder.Text))
            {
                MessageBox.Show(this, "The selected path is invalid. Please enter a valid windows path.",
                    "Download Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void txtCompleteFolder_Validating(object sender, CancelEventArgs e)
        {
            if (!Tools.IsValidPath(txtCompleteFolder.Text))
            {
                MessageBox.Show(this, "The selected path is invalid. Please enter a valid windows path.",
                    "Podcast Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }
    }
}