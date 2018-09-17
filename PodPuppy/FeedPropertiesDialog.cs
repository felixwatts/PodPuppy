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

namespace PodPuppy
{
    public partial class FeedPropertiesDialog : CentredDialog
    {
        private Item _exampleProvider;

        public FeedPropertiesDialog()
        {
            InitializeComponent();

            _cmbArchiveMode.BeginUpdate();
            foreach (ArchiveMode mode in Enum.GetValues(typeof(ArchiveMode)))
                _cmbArchiveMode.Items.Add(ArchiveModeToString(mode));
            _cmbArchiveMode.EndUpdate();

            if(Statics.Config != null) // needed for designer :S
                _cmbItemFilename.Items.AddRange(Statics.Config.SavedFilenameTags.ToArray());
        }

        public virtual void Populate(Feed feed)
        {
            _txtPodcastTitle.Text = feed.Title;
            _txtSaveFolder.Text = feed.Folder;
            _chkSyncronize.Checked = feed.Syncronised;
            _txtUrl.Text = feed.URL;
            _cmbArchiveMode.SelectedItem = ArchiveModeToString(feed.ArchiveMode);
            _cmbItemFilename.Text = feed.ItemFilenamePattern;

            _feedTagsControl.Populate(feed);

            _tabControl.SelectedTab = _tabBasics;

            Item[] items = feed.Items;
            _exampleProvider = items.Length == 0 ? null : items[0];
     
        }

        public virtual void Apply(Feed to)
        {
            to.URL = _txtUrl.Text;
            to.Title = _txtPodcastTitle.Text;
            to.Syncronised = _chkSyncronize.Checked;
            to.Folder = _txtSaveFolder.Text;
            _feedTagsControl.Apply(to);
            to.ArchiveMode = StringToArchiveMode(_cmbArchiveMode.SelectedItem as string);
            to.ItemFilenamePattern = _cmbItemFilename.Text;
        }

        private string ArchiveModeToString(ArchiveMode mode)
        {
            switch (mode)
            {
                case ArchiveMode.DeleteAfterOneWeek:
                    return "Delete Items After a Week";
                case ArchiveMode.DeleteAfterOneMonth:
                    return "Delete Items After a Month";
                case ArchiveMode.Keep:
                    return "Keep All";
                case ArchiveMode.KeepLatest:
                    return "Just Keep Latest Item";
                case ArchiveMode.MatchFeed:
                    return "Match Feed";
                default: throw new NotImplementedException("Cannot convert ArchiveMode to string: " + mode);
            }
        }

        protected ArchiveMode StringToArchiveMode(string str)
        {
            switch (str)
            {
                case "Delete Items After a Week":
                    return ArchiveMode.DeleteAfterOneWeek;
                case "Delete Items After a Month":
                    return ArchiveMode.DeleteAfterOneMonth;
                case "Keep All":
                    return ArchiveMode.Keep;
                case "Just Keep Latest Item":
                    return ArchiveMode.KeepLatest;
                case "Match Feed":
                    return ArchiveMode.MatchFeed;
                default: throw new NotImplementedException("Cannot convert string to ArchiveMode: " + str);
            }
        }

        private void _btnBrowseDownloadFolder_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.SelectedPath = Statics.Config.CompletedFilesBaseDirectory;
            if (_folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                _txtSaveFolder.Text = _folderBrowserDialog.SelectedPath;            
        }

        private void _btnSaveFilenameTag_Click(object sender, EventArgs e)
        {
            if (!Statics.Config.SavedFilenameTags.Contains(_cmbItemFilename.Text))
            {
                Statics.Config.SavedFilenameTags.Add(_cmbItemFilename.Text);
                _cmbItemFilename.Items.Add(_cmbItemFilename.Text);

                Statics.Config.Save();
            }
        }

        protected virtual void OnCmbArchiveModeSelectedIndexChanged(object sender, EventArgs e)
        {
        }        

        bool _updatingTooltip = false;
        private void _toolTip_Popup(object sender, PopupEventArgs e)
        {
            if (_updatingTooltip)
                return;
            _updatingTooltip = true;

            if (e.AssociatedControl == _cmbItemFilename)
            {
                if (_exampleProvider == null || !e.AssociatedControl.Text.Contains("%"))
                    e.Cancel = true;
                else _toolTip.SetToolTip(e.AssociatedControl, 
                    "Example: " + Statics.Config.GetCompleteDestination(_exampleProvider, e.AssociatedControl.Text));
            }

            _updatingTooltip = false;
        }

        private void _lnkTagHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.ShowTagHelp();
        }

        private bool IsValid()
        {
            bool valid = true;

            if (!Tools.IsValidPath(_txtSaveFolder.Text))
            {
                MessageBox.Show(this, "Please enter a valid windows path for the download folder.",
                    "Subscribe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!Directory.Exists(_txtSaveFolder.Text))
            {
                if (MessageBox.Show(this, "The following folder does not exist, do you want to create it?\n\n" + _txtSaveFolder.Text,
                    "Subscribe", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        Directory.CreateDirectory(_txtSaveFolder.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Unable to create podcast directory. " + ex.Message,
                            "Subscribe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void _btnSubscribe_Click(object sender, EventArgs e)
        {
            if(IsValid())
                DialogResult = DialogResult.OK;
        }
    }
}