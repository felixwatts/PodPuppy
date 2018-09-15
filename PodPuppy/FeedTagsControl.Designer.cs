namespace PodPuppy
{
    partial class FeedTagsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedTagsControl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkOverwriteExistingTags = new System.Windows.Forms.CheckBox();
            this._cmbTrackTitle = new System.Windows.Forms.ComboBox();
            this._cmbAlbum = new System.Windows.Forms.ComboBox();
            this._cmbArtist = new System.Windows.Forms.ComboBox();
            this._cmbGenre = new System.Windows.Forms.ComboBox();
            this._btnSaveAlbumTag = new System.Windows.Forms.Button();
            this._btnSaveTitleTag = new System.Windows.Forms.Button();
            this._btnSaveArtistTag = new System.Windows.Forms.Button();
            this._btnSaveGenreTag = new System.Windows.Forms.Button();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this._lnkTagHelp = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Album";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Artist";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Genre";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Track Title";
            // 
            // chkOverwriteExistingTags
            // 
            this.chkOverwriteExistingTags.AutoSize = true;
            this.chkOverwriteExistingTags.Location = new System.Drawing.Point(5, 131);
            this.chkOverwriteExistingTags.Name = "chkOverwriteExistingTags";
            this.chkOverwriteExistingTags.Size = new System.Drawing.Size(137, 17);
            this.chkOverwriteExistingTags.TabIndex = 8;
            this.chkOverwriteExistingTags.Text = "Overwrite Existing Tags";
            this.chkOverwriteExistingTags.UseVisualStyleBackColor = true;
            // 
            // _cmbTrackTitle
            // 
            this._cmbTrackTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbTrackTitle.FormattingEnabled = true;
            this._cmbTrackTitle.Location = new System.Drawing.Point(76, 3);
            this._cmbTrackTitle.Name = "_cmbTrackTitle";
            this._cmbTrackTitle.Size = new System.Drawing.Size(275, 21);
            this._cmbTrackTitle.TabIndex = 0;
            this._toolTip.SetToolTip(this._cmbTrackTitle, "x");
            // 
            // _cmbAlbum
            // 
            this._cmbAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbAlbum.FormattingEnabled = true;
            this._cmbAlbum.Location = new System.Drawing.Point(76, 29);
            this._cmbAlbum.Name = "_cmbAlbum";
            this._cmbAlbum.Size = new System.Drawing.Size(275, 21);
            this._cmbAlbum.TabIndex = 2;
            this._toolTip.SetToolTip(this._cmbAlbum, "x");
            // 
            // _cmbArtist
            // 
            this._cmbArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbArtist.FormattingEnabled = true;
            this._cmbArtist.Location = new System.Drawing.Point(76, 55);
            this._cmbArtist.Name = "_cmbArtist";
            this._cmbArtist.Size = new System.Drawing.Size(275, 21);
            this._cmbArtist.TabIndex = 4;
            this._toolTip.SetToolTip(this._cmbArtist, "x");
            // 
            // _cmbGenre
            // 
            this._cmbGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbGenre.FormattingEnabled = true;
            this._cmbGenre.Location = new System.Drawing.Point(76, 81);
            this._cmbGenre.Name = "_cmbGenre";
            this._cmbGenre.Size = new System.Drawing.Size(275, 21);
            this._cmbGenre.TabIndex = 6;
            this._toolTip.SetToolTip(this._cmbGenre, "x");
            // 
            // _btnSaveAlbumTag
            // 
            this._btnSaveAlbumTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSaveAlbumTag.Image = ((System.Drawing.Image)(resources.GetObject("_btnSaveAlbumTag.Image")));
            this._btnSaveAlbumTag.Location = new System.Drawing.Point(357, 27);
            this._btnSaveAlbumTag.Name = "_btnSaveAlbumTag";
            this._btnSaveAlbumTag.Size = new System.Drawing.Size(23, 23);
            this._btnSaveAlbumTag.TabIndex = 3;
            this._toolTip.SetToolTip(this._btnSaveAlbumTag, "Save Tag");
            this._btnSaveAlbumTag.UseVisualStyleBackColor = true;
            this._btnSaveAlbumTag.Click += new System.EventHandler(this.OnSaveTagClick);
            // 
            // _btnSaveTitleTag
            // 
            this._btnSaveTitleTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSaveTitleTag.Image = ((System.Drawing.Image)(resources.GetObject("_btnSaveTitleTag.Image")));
            this._btnSaveTitleTag.Location = new System.Drawing.Point(357, 1);
            this._btnSaveTitleTag.Name = "_btnSaveTitleTag";
            this._btnSaveTitleTag.Size = new System.Drawing.Size(23, 23);
            this._btnSaveTitleTag.TabIndex = 1;
            this._toolTip.SetToolTip(this._btnSaveTitleTag, "Save Tag");
            this._btnSaveTitleTag.UseVisualStyleBackColor = true;
            this._btnSaveTitleTag.Click += new System.EventHandler(this.OnSaveTagClick);
            // 
            // _btnSaveArtistTag
            // 
            this._btnSaveArtistTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSaveArtistTag.Image = ((System.Drawing.Image)(resources.GetObject("_btnSaveArtistTag.Image")));
            this._btnSaveArtistTag.Location = new System.Drawing.Point(357, 53);
            this._btnSaveArtistTag.Name = "_btnSaveArtistTag";
            this._btnSaveArtistTag.Size = new System.Drawing.Size(23, 23);
            this._btnSaveArtistTag.TabIndex = 5;
            this._toolTip.SetToolTip(this._btnSaveArtistTag, "Save Tag");
            this._btnSaveArtistTag.UseVisualStyleBackColor = true;
            this._btnSaveArtistTag.Click += new System.EventHandler(this.OnSaveTagClick);
            // 
            // _btnSaveGenreTag
            // 
            this._btnSaveGenreTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSaveGenreTag.Image = ((System.Drawing.Image)(resources.GetObject("_btnSaveGenreTag.Image")));
            this._btnSaveGenreTag.Location = new System.Drawing.Point(357, 79);
            this._btnSaveGenreTag.Name = "_btnSaveGenreTag";
            this._btnSaveGenreTag.Size = new System.Drawing.Size(23, 23);
            this._btnSaveGenreTag.TabIndex = 7;
            this._toolTip.SetToolTip(this._btnSaveGenreTag, "Save Tag");
            this._btnSaveGenreTag.UseVisualStyleBackColor = true;
            this._btnSaveGenreTag.Click += new System.EventHandler(this.OnSaveTagClick);
            // 
            // _toolTip
            // 
            this._toolTip.AutoPopDelay = 50000;
            this._toolTip.InitialDelay = 500;
            this._toolTip.ReshowDelay = 100;
            this._toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "%t = item title, %p = podcast title, %d = date";
            // 
            // _lnkTagHelp
            // 
            this._lnkTagHelp.AutoSize = true;
            this._lnkTagHelp.Location = new System.Drawing.Point(289, 105);
            this._lnkTagHelp.Name = "_lnkTagHelp";
            this._lnkTagHelp.Size = new System.Drawing.Size(39, 13);
            this._lnkTagHelp.TabIndex = 13;
            this._lnkTagHelp.TabStop = true;
            this._lnkTagHelp.Text = "more...";
            this._lnkTagHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkTagHelp_LinkClicked);
            // 
            // FeedTagsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._lnkTagHelp);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._btnSaveGenreTag);
            this.Controls.Add(this._btnSaveArtistTag);
            this.Controls.Add(this._btnSaveTitleTag);
            this.Controls.Add(this._btnSaveAlbumTag);
            this.Controls.Add(this._cmbGenre);
            this.Controls.Add(this._cmbArtist);
            this.Controls.Add(this._cmbAlbum);
            this.Controls.Add(this._cmbTrackTitle);
            this.Controls.Add(this.chkOverwriteExistingTags);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FeedTagsControl";
            this.Size = new System.Drawing.Size(386, 151);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkOverwriteExistingTags;
        private System.Windows.Forms.ComboBox _cmbTrackTitle;
        private System.Windows.Forms.ComboBox _cmbAlbum;
        private System.Windows.Forms.ComboBox _cmbArtist;
        private System.Windows.Forms.ComboBox _cmbGenre;
        private System.Windows.Forms.Button _btnSaveAlbumTag;
        private System.Windows.Forms.Button _btnSaveTitleTag;
        private System.Windows.Forms.Button _btnSaveArtistTag;
        private System.Windows.Forms.Button _btnSaveGenreTag;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel _lnkTagHelp;
    }
}
