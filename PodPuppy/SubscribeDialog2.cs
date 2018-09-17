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

namespace PodPuppy
{
    public partial class SubscribeDialog2 : FeedPropertiesDialog
    {
        public SubscribeDialog2()
        {
            InitializeComponent();
        }

        public override void Populate(Feed feed)
        {
            base.Populate(feed);

            _listItems.BeginUpdate();
            _listItems.Items.Clear();
            _listItems.Items.AddRange(feed.Items);
            foreach (Item item in feed.Items)
                item.Checked = true;
            _listItems.EndUpdate();
        }

        public override void Apply(Feed to)
        {
            base.Apply(to);

            foreach (Item item in to.Items)
            {
                if (!item.Checked)
                    item.Hide();
            }
        }

        protected override void OnCmbArchiveModeSelectedIndexChanged(object sender, EventArgs e)
        {
            base.OnCmbArchiveModeSelectedIndexChanged(sender, e);

            switch (StringToArchiveMode(_cmbArchiveMode.SelectedItem as string))
            {
                case ArchiveMode.DeleteAfterOneWeek:
                case ArchiveMode.DeleteAfterOneMonth:
                case ArchiveMode.Keep:
                case ArchiveMode.MatchFeed:
                    _listItems.Enabled = true;
                    break;
                case ArchiveMode.KeepLatest:
                    _listItems.Enabled = false;
                    SelectLatestItem();
                    break;
            }
        }

        private void SelectAllItems()
        {
            _listItems.BeginUpdate();
            foreach (Item item in _listItems.Items)
                item.Checked = true;
            _listItems.EndUpdate();
        }

        private void SelectNoItems()
        {
            _listItems.BeginUpdate();
            foreach (Item item in _listItems.Items)
                item.Checked = false;
            _listItems.EndUpdate();
        }

        private void SelectLatestItem()
        {
            if (_listItems.Items.Count == 0)
                return;

            _listItems.BeginUpdate();

            Item latest = (Item)_listItems.Items[0];
            foreach (Item item in _listItems.Items)
            {
                if (item.PublicationDate > latest.PublicationDate)
                    latest = item;
                item.Checked = false;
            }

            latest.Checked = true;

            _listItems.EndUpdate();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllItems();
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectNoItems();
        }

        private void selectLatestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectLatestItem();
        }

        private void SubscribeDialog2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _listItems.Items.Clear();
        }
    }
}
