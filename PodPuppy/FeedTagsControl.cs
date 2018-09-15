using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public enum TagCategory
    {
        Title,
        Artist,
        Album,
        Genre
    }

    public partial class FeedTagsControl : UserControl
    {
        private Item _exampleProvider;

        public FeedTagsControl()
        {
            InitializeComponent();

            if (Statics.Config != null)
            {
                _cmbTrackTitle.Items.AddRange(Statics.Config.SavedTitleTags.ToArray());
                _cmbAlbum.Items.AddRange(Statics.Config.SavedAlbumTags.ToArray());
                _cmbArtist.Items.AddRange(Statics.Config.SavedArtistTags.ToArray());
                _cmbGenre.Items.AddRange(Statics.Config.SavedGenreTags.ToArray());
            }
        }

        public void Populate(Feed feed)
        {
            if (!_cmbTrackTitle.Items.Contains(feed.TrackTitleTag))
                _cmbTrackTitle.Items.Add(feed.TrackTitleTag);
            _cmbTrackTitle.SelectedItem = feed.TrackTitleTag;

            if (!_cmbAlbum.Items.Contains(feed.AlbumTag))
                _cmbAlbum.Items.Add(feed.AlbumTag);
            _cmbAlbum.SelectedItem = feed.AlbumTag;

            if (!_cmbArtist.Items.Contains(feed.ArtistTag))
                _cmbArtist.Items.Add(feed.ArtistTag);
            _cmbArtist.SelectedItem = feed.ArtistTag;

            if (!_cmbGenre.Items.Contains(feed.GenreTag))
                _cmbGenre.Items.Add(feed.GenreTag);
            _cmbGenre.SelectedItem = feed.GenreTag;
            
            chkOverwriteExistingTags.Checked = feed.OverwriteExistingTags;

            Item[] items = feed.Items;
            _exampleProvider = items.Length == 0 ? null : items[0];
        }

        public void Apply(Feed feed)
        {
            //feed.TrackTitleTag = _cmbTrackTitle.SelectedItem as string;
            //feed.AlbumTag = _cmbAlbum.SelectedItem as string;
            //feed.ArtistTag = _cmbArtist.SelectedItem as string;
            //feed.GenreTag = _cmbGenre.SelectedItem as string;

            feed.TrackTitleTag = _cmbTrackTitle.Text;
            feed.AlbumTag = _cmbAlbum.Text;
            feed.ArtistTag = _cmbArtist.Text;
            feed.GenreTag = _cmbGenre.Text;

            feed.ApplyArtistTag = feed.ArtistTag != "";
            feed.ApplyAlbumTag = feed.AlbumTag != "";
            feed.ApplyGenreTag = feed.GenreTag != "";
            feed.ApplyTrackTitleTag = feed.TrackTitleTag != "";

            feed.OverwriteExistingTags = chkOverwriteExistingTags.Checked;
        }

        private void OnSaveTagClick(object sender, EventArgs e)
        {
            if (sender.Equals(_btnSaveAlbumTag))
            {
                if (!Statics.Config.SavedAlbumTags.Contains(_cmbAlbum.Text))
                {
                    Statics.Config.SavedAlbumTags.Add(_cmbAlbum.Text);
                    _cmbAlbum.Items.Add(_cmbAlbum.Text);
                }
            }
            else if (sender.Equals(_btnSaveGenreTag))
            {
                if (!Statics.Config.SavedGenreTags.Contains(_cmbGenre.Text))
                {
                    Statics.Config.SavedGenreTags.Add(_cmbGenre.Text);
                    _cmbGenre.Items.Add(_cmbGenre.Text);
                }
            }
            else if (sender.Equals(_btnSaveArtistTag))
            {
                if (!Statics.Config.SavedArtistTags.Contains(_cmbArtist.Text))
                {
                    Statics.Config.SavedArtistTags.Add(_cmbArtist.Text);
                    _cmbArtist.Items.Add(_cmbArtist.Text);
                }
            }
            else if (sender.Equals(_btnSaveTitleTag))
            {
                if (!Statics.Config.SavedTitleTags.Contains(_cmbTrackTitle.Text))
                {               
                    Statics.Config.SavedTitleTags.Add(_cmbTrackTitle.Text);
                    _cmbTrackTitle.Items.Add(_cmbTrackTitle.Text);
                }
            }

            Statics.Config.Save();
        }

        private bool _updatingTooltip = false;
        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            if (_updatingTooltip)
                return;
            _updatingTooltip = true;

            if (e.AssociatedControl is ComboBox)
            {
                if (_exampleProvider == null || !e.AssociatedControl.Text.Contains("%"))
                    e.Cancel = true;
                else _toolTip.SetToolTip(e.AssociatedControl, "Example: " + _exampleProvider.ExpandTagString(e.AssociatedControl.Text));
            }

            _updatingTooltip = false;
        }

        private void _lnkTagHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.ShowTagHelp();
        }
    }
}
