namespace PodPuppy
{
    partial class FeedPropertiesDialog
    {

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedPropertiesDialog));
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabBasics = new System.Windows.Forms.TabPage();
            this._lnkTagHelp = new System.Windows.Forms.LinkLabel();
            this._chkSyncronize = new System.Windows.Forms.CheckBox();
            this._btnBrowseDownloadFolder = new System.Windows.Forms.Button();
            this._btnSaveFilenameTag = new System.Windows.Forms.Button();
            this._cmbItemFilename = new System.Windows.Forms.ComboBox();
            this._cmbArchiveMode = new System.Windows.Forms.ComboBox();
            this._txtUrl = new System.Windows.Forms.TextBox();
            this._txtPodcastTitle = new System.Windows.Forms.TextBox();
            this._txtSaveFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._tabTags = new System.Windows.Forms.TabPage();
            this._feedTagsControl = new PodPuppy.FeedTagsControl();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnSubscribe = new System.Windows.Forms.Button();
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._tabControl.SuspendLayout();
            this._tabBasics.SuspendLayout();
            this._tabTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabBasics);
            this._tabControl.Controls.Add(this._tabTags);
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(522, 239);
            this._tabControl.TabIndex = 0;
            // 
            // _tabBasics
            // 
            this._tabBasics.Controls.Add(this._lnkTagHelp);
            this._tabBasics.Controls.Add(this._chkSyncronize);
            this._tabBasics.Controls.Add(this._btnBrowseDownloadFolder);
            this._tabBasics.Controls.Add(this._btnSaveFilenameTag);
            this._tabBasics.Controls.Add(this._cmbItemFilename);
            this._tabBasics.Controls.Add(this._cmbArchiveMode);
            this._tabBasics.Controls.Add(this._txtUrl);
            this._tabBasics.Controls.Add(this._txtPodcastTitle);
            this._tabBasics.Controls.Add(this._txtSaveFolder);
            this._tabBasics.Controls.Add(this.label5);
            this._tabBasics.Controls.Add(this.label6);
            this._tabBasics.Controls.Add(this.label2);
            this._tabBasics.Controls.Add(this.label4);
            this._tabBasics.Controls.Add(this.label3);
            this._tabBasics.Controls.Add(this.label1);
            this._tabBasics.Location = new System.Drawing.Point(4, 22);
            this._tabBasics.Name = "_tabBasics";
            this._tabBasics.Padding = new System.Windows.Forms.Padding(3);
            this._tabBasics.Size = new System.Drawing.Size(514, 213);
            this._tabBasics.TabIndex = 0;
            this._tabBasics.Text = "Basics";
            this._tabBasics.UseVisualStyleBackColor = true;
            // 
            // _lnkTagHelp
            // 
            this._lnkTagHelp.AutoSize = true;
            this._lnkTagHelp.Location = new System.Drawing.Point(358, 113);
            this._lnkTagHelp.Name = "_lnkTagHelp";
            this._lnkTagHelp.Size = new System.Drawing.Size(39, 13);
            this._lnkTagHelp.TabIndex = 7;
            this._lnkTagHelp.TabStop = true;
            this._lnkTagHelp.Text = "more...";
            this._lnkTagHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnkTagHelp_LinkClicked);
            // 
            // _chkSyncronize
            // 
            this._chkSyncronize.AutoSize = true;
            this._chkSyncronize.Location = new System.Drawing.Point(91, 165);
            this._chkSyncronize.Name = "_chkSyncronize";
            this._chkSyncronize.Size = new System.Drawing.Size(169, 17);
            this._chkSyncronize.TabIndex = 6;
            this._chkSyncronize.Text = "Syncronize to Portable Device";
            this._chkSyncronize.UseVisualStyleBackColor = true;
            // 
            // _btnBrowseDownloadFolder
            // 
            this._btnBrowseDownloadFolder.Image = global::PodPuppy.Properties.Resources.IconFolder;
            this._btnBrowseDownloadFolder.Location = new System.Drawing.Point(483, 62);
            this._btnBrowseDownloadFolder.Name = "_btnBrowseDownloadFolder";
            this._btnBrowseDownloadFolder.Size = new System.Drawing.Size(23, 23);
            this._btnBrowseDownloadFolder.TabIndex = 2;
            this._toolTip.SetToolTip(this._btnBrowseDownloadFolder, "Browse");
            this._btnBrowseDownloadFolder.UseVisualStyleBackColor = true;
            this._btnBrowseDownloadFolder.Click += new System.EventHandler(this._btnBrowseDownloadFolder_Click);
            // 
            // _btnSaveFilenameTag
            // 
            this._btnSaveFilenameTag.Image = global::PodPuppy.Properties.Resources.add2_small;
            this._btnSaveFilenameTag.Location = new System.Drawing.Point(483, 89);
            this._btnSaveFilenameTag.Name = "_btnSaveFilenameTag";
            this._btnSaveFilenameTag.Size = new System.Drawing.Size(23, 23);
            this._btnSaveFilenameTag.TabIndex = 4;
            this._toolTip.SetToolTip(this._btnSaveFilenameTag, "Save Pattern");
            this._btnSaveFilenameTag.UseVisualStyleBackColor = true;
            this._btnSaveFilenameTag.Click += new System.EventHandler(this._btnSaveFilenameTag_Click);
            // 
            // _cmbItemFilename
            // 
            this._cmbItemFilename.FormattingEnabled = true;
            this._cmbItemFilename.Location = new System.Drawing.Point(91, 89);
            this._cmbItemFilename.Name = "_cmbItemFilename";
            this._cmbItemFilename.Size = new System.Drawing.Size(386, 21);
            this._cmbItemFilename.TabIndex = 3;
            this._toolTip.SetToolTip(this._cmbItemFilename, "x");
            // 
            // _cmbArchiveMode
            // 
            this._cmbArchiveMode.FormattingEnabled = true;
            this._cmbArchiveMode.Location = new System.Drawing.Point(91, 138);
            this._cmbArchiveMode.Name = "_cmbArchiveMode";
            this._cmbArchiveMode.Size = new System.Drawing.Size(157, 21);
            this._cmbArchiveMode.TabIndex = 5;
            this._cmbArchiveMode.SelectedIndexChanged += new System.EventHandler(this.OnCmbArchiveModeSelectedIndexChanged);
            // 
            // _txtUrl
            // 
            this._txtUrl.Location = new System.Drawing.Point(91, 12);
            this._txtUrl.Name = "_txtUrl";
            this._txtUrl.Size = new System.Drawing.Size(386, 20);
            this._txtUrl.TabIndex = 0;
            // 
            // _txtPodcastTitle
            // 
            this._txtPodcastTitle.Location = new System.Drawing.Point(91, 38);
            this._txtPodcastTitle.Name = "_txtPodcastTitle";
            this._txtPodcastTitle.Size = new System.Drawing.Size(386, 20);
            this._txtPodcastTitle.TabIndex = 0;
            // 
            // _txtSaveFolder
            // 
            this._txtSaveFolder.Location = new System.Drawing.Point(91, 64);
            this._txtSaveFolder.Name = "_txtSaveFolder";
            this._txtSaveFolder.Size = new System.Drawing.Size(386, 20);
            this._txtSaveFolder.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(88, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(264, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "%t = item title, %p = podcast title, %d = publication date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Archive Mode ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Podcast Title";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Item Filenames";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Save Folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Feed URL";
            // 
            // _tabTags
            // 
            this._tabTags.Controls.Add(this._feedTagsControl);
            this._tabTags.Location = new System.Drawing.Point(4, 22);
            this._tabTags.Name = "_tabTags";
            this._tabTags.Padding = new System.Windows.Forms.Padding(3);
            this._tabTags.Size = new System.Drawing.Size(514, 213);
            this._tabTags.TabIndex = 1;
            this._tabTags.Text = "Tagging";
            this._tabTags.UseVisualStyleBackColor = true;
            // 
            // _feedTagsControl
            // 
            this._feedTagsControl.Location = new System.Drawing.Point(6, 18);
            this._feedTagsControl.Name = "_feedTagsControl";
            this._feedTagsControl.Size = new System.Drawing.Size(500, 151);
            this._feedTagsControl.TabIndex = 0;
            // 
            // _btnCancel
            // 
            this._btnCancel.CausesValidation = false;
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(435, 245);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _btnSubscribe
            // 
            this._btnSubscribe.Location = new System.Drawing.Point(354, 245);
            this._btnSubscribe.Name = "_btnSubscribe";
            this._btnSubscribe.Size = new System.Drawing.Size(75, 23);
            this._btnSubscribe.TabIndex = 1;
            this._btnSubscribe.Text = "OK";
            this._btnSubscribe.UseVisualStyleBackColor = true;
            this._btnSubscribe.Click += new System.EventHandler(this._btnSubscribe_Click);
            // 
            // _toolTip
            // 
            this._toolTip.AutoPopDelay = 50000;
            this._toolTip.InitialDelay = 500;
            this._toolTip.ReshowDelay = 100;
            this._toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this._toolTip_Popup);
            // 
            // FeedPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 277);
            this.ControlBox = false;
            this.Controls.Add(this._btnSubscribe);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FeedPropertiesDialog";
            this.Text = "Feed Properties";
            this._tabControl.ResumeLayout(false);
            this._tabBasics.ResumeLayout(false);
            this._tabBasics.PerformLayout();
            this._tabTags.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage _tabBasics;
        private System.Windows.Forms.TabPage _tabTags;
        private System.Windows.Forms.TextBox _txtSaveFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.ComboBox _cmbArchiveMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private FeedTagsControl _feedTagsControl;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ComboBox _cmbItemFilename;
        private System.Windows.Forms.Button _btnSaveFilenameTag;
        private System.Windows.Forms.Button _btnBrowseDownloadFolder;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        private System.Windows.Forms.TextBox _txtPodcastTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox _chkSyncronize;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.LinkLabel _lnkTagHelp;
        private System.Windows.Forms.TextBox _txtUrl;
        protected System.Windows.Forms.TabControl _tabControl;
        protected System.Windows.Forms.Button _btnSubscribe;
        private System.ComponentModel.IContainer components;
    }
}