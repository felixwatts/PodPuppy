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
    public partial class ItemTagsDlg : CentredDialog
    {
        private Item _item;
        private bool _unsavedChanges;

        public ItemTagsDlg(Item item)
        {
            InitializeComponent();

            _item = item;
            Populate();
        }

        private void Populate()
        {
            txtTrackTitleTag.Text = _item.TrackTitleTag;
            txtAlbumTag.Text = _item.AlbumTag;
            txtGenreTag.Text = _item.GenreTag;
            txtArtistTag.Text = _item.ArtistTag;
            _unsavedChanges = false;
        }

        private void Apply()
        {
            if (_unsavedChanges)
            {
                _item.ArtistTag = txtArtistTag.Text;
                _item.AlbumTag = txtAlbumTag.Text;
                _item.GenreTag = txtGenreTag.Text;
                _item.TrackTitleTag = txtTrackTitleTag.Text;
                _unsavedChanges = !_item.WriteTagsToFile(true);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            _unsavedChanges = true;
        }
    }
}