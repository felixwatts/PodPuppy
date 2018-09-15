namespace PodPuppy
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.syncProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lnkCancelSync = new System.Windows.Forms.ToolStripStatusLabel();
            this.listFeeds = new PodPuppy.ListViewNoFlicker();
            this.columnHeaderTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSynced = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPriority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLatest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuFeed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuFeedRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFeedRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFeedOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFeedPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFeedProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFeedCopyUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuIconRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIconPause = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIconResume = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIconOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuIconExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnRefreshAll = new System.Windows.Forms.ToolStripButton();
            this.btnOpenPodcastsFolder = new System.Windows.Forms.ToolStripButton();
            this.btnResumeDownloads = new System.Windows.Forms.ToolStripButton();
            this.btnPauseDownloads = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveSelectedFeeds = new System.Windows.Forms.ToolStripButton();
            this.btnRaisePriority = new System.Windows.Forms.ToolStripButton();
            this.btnLowerPriority = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.listItems = new PodPuppy.ListViewNoFlicker();
            this.columnHeaderItemTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderItemStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderItemPubDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderItemDownloadDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSkipSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUnskipSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTags = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTryAgain = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileSyncronise = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPodcasts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPodcastsPause = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPodcastsResume = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPodcastsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainPodcastsOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelpUserGuide = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelpVisitWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip.SuspendLayout();
            this.menuFeed.SuspendLayout();
            this.menuIcon.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.menuItem.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.syncProgressBar,
            this.lnkCancelSync});
            this.statusStrip.Location = new System.Drawing.Point(0, 395);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(717, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(702, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // syncProgressBar
            // 
            this.syncProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.syncProgressBar.AutoSize = false;
            this.syncProgressBar.Name = "syncProgressBar";
            this.syncProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.syncProgressBar.Size = new System.Drawing.Size(100, 16);
            this.syncProgressBar.Visible = false;
            // 
            // lnkCancelSync
            // 
            this.lnkCancelSync.IsLink = true;
            this.lnkCancelSync.Name = "lnkCancelSync";
            this.lnkCancelSync.Size = new System.Drawing.Size(43, 17);
            this.lnkCancelSync.Text = "Cancel";
            this.lnkCancelSync.Visible = false;
            this.lnkCancelSync.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // listFeeds
            // 
            this.listFeeds.AllowDrop = true;
            this.listFeeds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTitle,
            this.columnHeaderStatus,
            this.columnHeaderSynced,
            this.columnHeaderPriority,
            this.columnHeaderLatest});
            this.listFeeds.ContextMenuStrip = this.menuFeed;
            this.listFeeds.Dock = System.Windows.Forms.DockStyle.Top;
            this.listFeeds.FullRowSelect = true;
            this.listFeeds.HideSelection = false;
            this.listFeeds.Location = new System.Drawing.Point(0, 0);
            this.listFeeds.Name = "listFeeds";
            this.listFeeds.ShowItemToolTips = true;
            this.listFeeds.Size = new System.Drawing.Size(717, 167);
            this.listFeeds.TabIndex = 4;
            this.listFeeds.UseCompatibleStateImageBehavior = false;
            this.listFeeds.View = System.Windows.Forms.View.Details;
            this.listFeeds.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listFeedsColumnClick);
            this.listFeeds.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listFeedsItemDrag);
            this.listFeeds.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listFeedsItemSelectionChanged);
            this.listFeeds.SelectedIndexChanged += new System.EventHandler(this.listFeedsSelectedIndexChanged);
            this.listFeeds.DragDrop += new System.Windows.Forms.DragEventHandler(this.listFeedsDragDrop);
            this.listFeeds.DragEnter += new System.Windows.Forms.DragEventHandler(this.listFeedsDragEnter);
            this.listFeeds.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listFeeds_KeyUp);
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "Podcast Title";
            this.columnHeaderTitle.Width = 300;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 156;
            // 
            // columnHeaderSynced
            // 
            this.columnHeaderSynced.Text = "Syncronise";
            this.columnHeaderSynced.Width = 68;
            // 
            // columnHeaderPriority
            // 
            this.columnHeaderPriority.Text = "Priority";
            // 
            // columnHeaderLatest
            // 
            this.columnHeaderLatest.Text = "Latest Item";
            this.columnHeaderLatest.Width = 100;
            // 
            // menuFeed
            // 
            this.menuFeed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFeedRefresh,
            this.menuFeedRemove,
            this.menuFeedOpenFolder,
            this.menuFeedPlay,
            this.menuFeedProperties,
            this.menuFeedCopyUrl});
            this.menuFeed.Name = "contextMenuListViewMainItem";
            this.menuFeed.Size = new System.Drawing.Size(202, 136);
            this.menuFeed.Opening += new System.ComponentModel.CancelEventHandler(this.menuFeedOpening);
            // 
            // menuFeedRefresh
            // 
            this.menuFeedRefresh.Image = ((System.Drawing.Image)(resources.GetObject("menuFeedRefresh.Image")));
            this.menuFeedRefresh.Name = "menuFeedRefresh";
            this.menuFeedRefresh.Size = new System.Drawing.Size(201, 22);
            this.menuFeedRefresh.Text = "Check for New Items";
            this.menuFeedRefresh.Click += new System.EventHandler(this.menuFeedRefreshClick);
            // 
            // menuFeedRemove
            // 
            this.menuFeedRemove.Image = ((System.Drawing.Image)(resources.GetObject("menuFeedRemove.Image")));
            this.menuFeedRemove.Name = "menuFeedRemove";
            this.menuFeedRemove.Size = new System.Drawing.Size(201, 22);
            this.menuFeedRemove.Text = "Unsubscribe";
            this.menuFeedRemove.Click += new System.EventHandler(this.RemoveSelectedFeedsClick);
            // 
            // menuFeedOpenFolder
            // 
            this.menuFeedOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("menuFeedOpenFolder.Image")));
            this.menuFeedOpenFolder.Name = "menuFeedOpenFolder";
            this.menuFeedOpenFolder.Size = new System.Drawing.Size(201, 22);
            this.menuFeedOpenFolder.Text = "Open Containing Folder";
            this.menuFeedOpenFolder.Click += new System.EventHandler(this.menuFeedOpenFolderClick);
            // 
            // menuFeedPlay
            // 
            this.menuFeedPlay.Image = ((System.Drawing.Image)(resources.GetObject("menuFeedPlay.Image")));
            this.menuFeedPlay.Name = "menuFeedPlay";
            this.menuFeedPlay.Size = new System.Drawing.Size(201, 22);
            this.menuFeedPlay.Text = "Play";
            this.menuFeedPlay.Click += new System.EventHandler(this.menuFeedPlayClick);
            // 
            // menuFeedProperties
            // 
            this.menuFeedProperties.Image = global::PodPuppy.Properties.Resources.tag;
            this.menuFeedProperties.Name = "menuFeedProperties";
            this.menuFeedProperties.Size = new System.Drawing.Size(201, 22);
            this.menuFeedProperties.Text = "Properties";
            this.menuFeedProperties.Click += new System.EventHandler(this.OnMenuFeedPropertiesClick);
            // 
            // menuFeedCopyUrl
            // 
            this.menuFeedCopyUrl.Image = ((System.Drawing.Image)(resources.GetObject("menuFeedCopyUrl.Image")));
            this.menuFeedCopyUrl.Name = "menuFeedCopyUrl";
            this.menuFeedCopyUrl.Size = new System.Drawing.Size(201, 22);
            this.menuFeedCopyUrl.Text = "Copy URL";
            this.menuFeedCopyUrl.Click += new System.EventHandler(this.menuFeedCopyUrlClick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.menuIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "PodPuppy";
            this.notifyIcon.Visible = true;
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIconBalloonTipClicked);
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIconDoubleClick);
            // 
            // menuIcon
            // 
            this.menuIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuIconRefresh,
            this.menuIconPause,
            this.menuIconResume,
            this.menuIconOpenFolder,
            this.toolStripMenuItem2,
            this.menuIconExit});
            this.menuIcon.Name = "contextMenuNotifyIcon";
            this.menuIcon.Size = new System.Drawing.Size(190, 120);
            // 
            // menuIconRefresh
            // 
            this.menuIconRefresh.Image = ((System.Drawing.Image)(resources.GetObject("menuIconRefresh.Image")));
            this.menuIconRefresh.Name = "menuIconRefresh";
            this.menuIconRefresh.Size = new System.Drawing.Size(189, 22);
            this.menuIconRefresh.Text = "Check Feeds Now";
            this.menuIconRefresh.Click += new System.EventHandler(this.RefreshAllClick);
            // 
            // menuIconPause
            // 
            this.menuIconPause.Image = ((System.Drawing.Image)(resources.GetObject("menuIconPause.Image")));
            this.menuIconPause.Name = "menuIconPause";
            this.menuIconPause.Size = new System.Drawing.Size(189, 22);
            this.menuIconPause.Text = "Pause Downloads";
            this.menuIconPause.Click += new System.EventHandler(this.PauseClick);
            // 
            // menuIconResume
            // 
            this.menuIconResume.Image = ((System.Drawing.Image)(resources.GetObject("menuIconResume.Image")));
            this.menuIconResume.Name = "menuIconResume";
            this.menuIconResume.Size = new System.Drawing.Size(189, 22);
            this.menuIconResume.Text = "Resume Downloads";
            this.menuIconResume.Click += new System.EventHandler(this.ResumeClick);
            // 
            // menuIconOpenFolder
            // 
            this.menuIconOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("menuIconOpenFolder.Image")));
            this.menuIconOpenFolder.Name = "menuIconOpenFolder";
            this.menuIconOpenFolder.Size = new System.Drawing.Size(189, 22);
            this.menuIconOpenFolder.Text = "Open Podcasts Folder";
            this.menuIconOpenFolder.Click += new System.EventHandler(this.OpenPodcastsFolderClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(186, 6);
            // 
            // menuIconExit
            // 
            this.menuIconExit.Name = "menuIconExit";
            this.menuIconExit.Size = new System.Drawing.Size(189, 22);
            this.menuIconExit.Text = "Exit";
            this.menuIconExit.Click += new System.EventHandler(this.ExitClick);
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshAll,
            this.btnOpenPodcastsFolder,
            this.btnResumeDownloads,
            this.btnPauseDownloads,
            this.btnRemoveSelectedFeeds,
            this.btnRaisePriority,
            this.btnLowerPriority,
            this.btnAdd,
            this.toolStripTextBox});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(717, 27);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 10;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.AccessibleDescription = "Check Podcasts for New Items";
            this.btnRefreshAll.AccessibleName = "Refresh All";
            this.btnRefreshAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefreshAll.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshAll.Image")));
            this.btnRefreshAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefreshAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Size = new System.Drawing.Size(24, 24);
            this.btnRefreshAll.Text = "Check Feeds for New Items";
            this.btnRefreshAll.Click += new System.EventHandler(this.RefreshAllClick);
            // 
            // btnOpenPodcastsFolder
            // 
            this.btnOpenPodcastsFolder.AccessibleDescription = "Open Podcasts Folder";
            this.btnOpenPodcastsFolder.AccessibleName = "Open Podcasts Folder";
            this.btnOpenPodcastsFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenPodcastsFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenPodcastsFolder.Image")));
            this.btnOpenPodcastsFolder.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOpenPodcastsFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenPodcastsFolder.Name = "btnOpenPodcastsFolder";
            this.btnOpenPodcastsFolder.Size = new System.Drawing.Size(23, 24);
            this.btnOpenPodcastsFolder.Text = "Open Podcasts Folder";
            this.btnOpenPodcastsFolder.Click += new System.EventHandler(this.OpenPodcastsFolderClick);
            // 
            // btnResumeDownloads
            // 
            this.btnResumeDownloads.AccessibleDescription = "Resume Downloads";
            this.btnResumeDownloads.AccessibleName = "Resume Downloads";
            this.btnResumeDownloads.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnResumeDownloads.Image = ((System.Drawing.Image)(resources.GetObject("btnResumeDownloads.Image")));
            this.btnResumeDownloads.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnResumeDownloads.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResumeDownloads.Name = "btnResumeDownloads";
            this.btnResumeDownloads.Size = new System.Drawing.Size(24, 24);
            this.btnResumeDownloads.Text = "Resume Downloads";
            this.btnResumeDownloads.Click += new System.EventHandler(this.ResumeClick);
            // 
            // btnPauseDownloads
            // 
            this.btnPauseDownloads.AccessibleDescription = "Pause Downloads";
            this.btnPauseDownloads.AccessibleName = "Pause Downloads";
            this.btnPauseDownloads.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPauseDownloads.Image = ((System.Drawing.Image)(resources.GetObject("btnPauseDownloads.Image")));
            this.btnPauseDownloads.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPauseDownloads.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPauseDownloads.Name = "btnPauseDownloads";
            this.btnPauseDownloads.Size = new System.Drawing.Size(24, 24);
            this.btnPauseDownloads.Text = "Pause Downloads";
            this.btnPauseDownloads.Click += new System.EventHandler(this.PauseClick);
            // 
            // btnRemoveSelectedFeeds
            // 
            this.btnRemoveSelectedFeeds.AccessibleName = "Usubscribe From Selected Feed";
            this.btnRemoveSelectedFeeds.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveSelectedFeeds.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveSelectedFeeds.Image")));
            this.btnRemoveSelectedFeeds.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRemoveSelectedFeeds.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveSelectedFeeds.Name = "btnRemoveSelectedFeeds";
            this.btnRemoveSelectedFeeds.Size = new System.Drawing.Size(24, 24);
            this.btnRemoveSelectedFeeds.Text = "Unsubscribe from Selected Podcasts";
            this.btnRemoveSelectedFeeds.Click += new System.EventHandler(this.RemoveSelectedFeedsClick);
            // 
            // btnRaisePriority
            // 
            this.btnRaisePriority.AccessibleName = "Raise Selected Feed Priority";
            this.btnRaisePriority.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRaisePriority.Image = ((System.Drawing.Image)(resources.GetObject("btnRaisePriority.Image")));
            this.btnRaisePriority.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRaisePriority.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRaisePriority.Name = "btnRaisePriority";
            this.btnRaisePriority.Size = new System.Drawing.Size(24, 24);
            this.btnRaisePriority.Text = "Raise Podcast Priority";
            this.btnRaisePriority.Click += new System.EventHandler(this.btnRaisePriorityClick);
            // 
            // btnLowerPriority
            // 
            this.btnLowerPriority.AccessibleName = "Lower Selected Feed Priority";
            this.btnLowerPriority.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLowerPriority.Image = ((System.Drawing.Image)(resources.GetObject("btnLowerPriority.Image")));
            this.btnLowerPriority.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLowerPriority.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLowerPriority.Name = "btnLowerPriority";
            this.btnLowerPriority.Size = new System.Drawing.Size(24, 24);
            this.btnLowerPriority.Text = "Lower Podcast Priority";
            this.btnLowerPriority.Click += new System.EventHandler(this.btnLowerPriorityClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Enabled = false;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 24);
            this.btnAdd.Text = "Add Subscription";
            this.btnAdd.ToolTipText = "Subscribe to Podcast";
            this.btnAdd.Click += new System.EventHandler(this.btnAddClick);
            // 
            // toolStripTextBox
            // 
            this.toolStripTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox.Name = "toolStripTextBox";
            this.toolStripTextBox.Size = new System.Drawing.Size(150, 27);
            this.toolStripTextBox.Text = " subscribe or search";
            this.toolStripTextBox.ToolTipText = "Enter the URL of a Podcast here to subscribe to it. Or enter some keywords to sea" +
    "rch google for related podcasts.";
            this.toolStripTextBox.Enter += new System.EventHandler(this.toolStripTextBoxEnter);
            this.toolStripTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBoxKeyUp);
            this.toolStripTextBox.TextChanged += new System.EventHandler(this.toolStripTextBoxTextChanged);
            // 
            // toolStripContainer2
            // 
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.listItems);
            this.toolStripContainer2.ContentPanel.Controls.Add(this.splitter1);
            this.toolStripContainer2.ContentPanel.Controls.Add(this.listFeeds);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(717, 344);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            this.toolStripContainer2.Size = new System.Drawing.Size(717, 371);
            this.toolStripContainer2.TabIndex = 12;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // listItems
            // 
            this.listItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderItemTitle,
            this.columnHeaderItemStatus,
            this.columnHeaderItemPubDate,
            this.columnHeaderItemDownloadDate});
            this.listItems.ContextMenuStrip = this.menuItem;
            this.listItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listItems.FullRowSelect = true;
            this.listItems.HideSelection = false;
            this.listItems.Location = new System.Drawing.Point(0, 177);
            this.listItems.Name = "listItems";
            this.listItems.ShowItemToolTips = true;
            this.listItems.Size = new System.Drawing.Size(717, 167);
            this.listItems.TabIndex = 6;
            this.listItems.UseCompatibleStateImageBehavior = false;
            this.listItems.View = System.Windows.Forms.View.Details;
            this.listItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listItemsColumnClick);
            this.listItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listItemsItemDrag);
            this.listItems.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listItemsItemSelectionChanged);
            this.listItems.DoubleClick += new System.EventHandler(this.listItemsDoubleClick);
            // 
            // columnHeaderItemTitle
            // 
            this.columnHeaderItemTitle.Text = "Track Title";
            this.columnHeaderItemTitle.Width = 208;
            // 
            // columnHeaderItemStatus
            // 
            this.columnHeaderItemStatus.Text = "Status";
            this.columnHeaderItemStatus.Width = 143;
            // 
            // columnHeaderItemPubDate
            // 
            this.columnHeaderItemPubDate.Text = "Published";
            this.columnHeaderItemPubDate.Width = 97;
            // 
            // columnHeaderItemDownloadDate
            // 
            this.columnHeaderItemDownloadDate.Text = "Downloaded";
            this.columnHeaderItemDownloadDate.Width = 100;
            // 
            // menuItem
            // 
            this.menuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPlay,
            this.menuItemOpenFolder,
            this.menuItemSkipSelected,
            this.menuItemUnskipSelected,
            this.menuItemTags,
            this.menuItemCopyUrl,
            this.menuItemViewInfo,
            this.menuItemTryAgain});
            this.menuItem.Name = "contextMenuListViewItemsItem";
            this.menuItem.Size = new System.Drawing.Size(202, 180);
            this.menuItem.Opening += new System.ComponentModel.CancelEventHandler(this.menuItemOpening);
            // 
            // menuItemPlay
            // 
            this.menuItemPlay.Image = ((System.Drawing.Image)(resources.GetObject("menuItemPlay.Image")));
            this.menuItemPlay.Name = "menuItemPlay";
            this.menuItemPlay.Size = new System.Drawing.Size(201, 22);
            this.menuItemPlay.Text = "Play";
            this.menuItemPlay.Click += new System.EventHandler(this.menuItemPlayClick);
            // 
            // menuItemOpenFolder
            // 
            this.menuItemOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("menuItemOpenFolder.Image")));
            this.menuItemOpenFolder.Name = "menuItemOpenFolder";
            this.menuItemOpenFolder.Size = new System.Drawing.Size(201, 22);
            this.menuItemOpenFolder.Text = "Open Containing Folder";
            this.menuItemOpenFolder.Click += new System.EventHandler(this.menuItemOpenFolderClick);
            // 
            // menuItemSkipSelected
            // 
            this.menuItemSkipSelected.Image = ((System.Drawing.Image)(resources.GetObject("menuItemSkipSelected.Image")));
            this.menuItemSkipSelected.Name = "menuItemSkipSelected";
            this.menuItemSkipSelected.Size = new System.Drawing.Size(201, 22);
            this.menuItemSkipSelected.Text = "Skip";
            this.menuItemSkipSelected.Click += new System.EventHandler(this.menuItemSkipSelectedClick);
            // 
            // menuItemUnskipSelected
            // 
            this.menuItemUnskipSelected.Image = ((System.Drawing.Image)(resources.GetObject("menuItemUnskipSelected.Image")));
            this.menuItemUnskipSelected.Name = "menuItemUnskipSelected";
            this.menuItemUnskipSelected.Size = new System.Drawing.Size(201, 22);
            this.menuItemUnskipSelected.Text = "Unskip";
            this.menuItemUnskipSelected.Click += new System.EventHandler(this.menuItemUnskipSelectedClick);
            // 
            // menuItemTags
            // 
            this.menuItemTags.Image = ((System.Drawing.Image)(resources.GetObject("menuItemTags.Image")));
            this.menuItemTags.Name = "menuItemTags";
            this.menuItemTags.Size = new System.Drawing.Size(201, 22);
            this.menuItemTags.Text = "Tags";
            this.menuItemTags.Click += new System.EventHandler(this.menuItemTags_Click);
            // 
            // menuItemCopyUrl
            // 
            this.menuItemCopyUrl.Image = ((System.Drawing.Image)(resources.GetObject("menuItemCopyUrl.Image")));
            this.menuItemCopyUrl.Name = "menuItemCopyUrl";
            this.menuItemCopyUrl.Size = new System.Drawing.Size(201, 22);
            this.menuItemCopyUrl.Text = "Copy URL";
            this.menuItemCopyUrl.Click += new System.EventHandler(this.menuItemCopyUrlClick);
            // 
            // menuItemViewInfo
            // 
            this.menuItemViewInfo.Image = ((System.Drawing.Image)(resources.GetObject("menuItemViewInfo.Image")));
            this.menuItemViewInfo.Name = "menuItemViewInfo";
            this.menuItemViewInfo.Size = new System.Drawing.Size(201, 22);
            this.menuItemViewInfo.Text = "View Info";
            this.menuItemViewInfo.Click += new System.EventHandler(this.menuItemViewInfoClick);
            // 
            // menuItemTryAgain
            // 
            this.menuItemTryAgain.Name = "menuItemTryAgain";
            this.menuItemTryAgain.Size = new System.Drawing.Size(201, 22);
            this.menuItemTryAgain.Text = "Try Again";
            this.menuItemTryAgain.Visible = false;
            this.menuItemTryAgain.Click += new System.EventHandler(this.menuItemTryAgainClick);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 167);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(717, 10);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 60000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimerTick);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFile,
            this.menuMainPodcasts,
            this.menuMainHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(717, 24);
            this.menuMain.TabIndex = 13;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuMainFile
            // 
            this.menuMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFileOptions,
            this.menuMainFileSyncronise,
            this.toolStripMenuItem3,
            this.menuMainFileImport,
            this.menuMainFileExport,
            this.toolStripMenuItem1,
            this.menuMainFileExit});
            this.menuMainFile.Name = "menuMainFile";
            this.menuMainFile.Size = new System.Drawing.Size(37, 20);
            this.menuMainFile.Text = "&File";
            // 
            // menuMainFileOptions
            // 
            this.menuMainFileOptions.Name = "menuMainFileOptions";
            this.menuMainFileOptions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuMainFileOptions.Size = new System.Drawing.Size(184, 22);
            this.menuMainFileOptions.Text = "&Options...";
            this.menuMainFileOptions.Click += new System.EventHandler(this.menuMainFileOptionsClick);
            // 
            // menuMainFileSyncronise
            // 
            this.menuMainFileSyncronise.Name = "menuMainFileSyncronise";
            this.menuMainFileSyncronise.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuMainFileSyncronise.Size = new System.Drawing.Size(184, 22);
            this.menuMainFileSyncronise.Text = "&Syncronise";
            this.menuMainFileSyncronise.Click += new System.EventHandler(this.menuMainFileSyncroniseClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // menuMainFileImport
            // 
            this.menuMainFileImport.Name = "menuMainFileImport";
            this.menuMainFileImport.Size = new System.Drawing.Size(184, 22);
            this.menuMainFileImport.Text = "Import Subscriptions";
            this.menuMainFileImport.Click += new System.EventHandler(this.menuMainFileImportClick);
            // 
            // menuMainFileExport
            // 
            this.menuMainFileExport.Name = "menuMainFileExport";
            this.menuMainFileExport.Size = new System.Drawing.Size(184, 22);
            this.menuMainFileExport.Text = "Export Subscriptions";
            this.menuMainFileExport.Click += new System.EventHandler(this.menuMainFileExportCLick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // menuMainFileExit
            // 
            this.menuMainFileExit.Name = "menuMainFileExit";
            this.menuMainFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuMainFileExit.Size = new System.Drawing.Size(184, 22);
            this.menuMainFileExit.Text = "E&xit";
            this.menuMainFileExit.Click += new System.EventHandler(this.ExitClick);
            // 
            // menuMainPodcasts
            // 
            this.menuMainPodcasts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainPodcastsPause,
            this.menuMainPodcastsResume,
            this.menuMainPodcastsRefresh,
            this.menuMainPodcastsOpenFolder});
            this.menuMainPodcasts.Name = "menuMainPodcasts";
            this.menuMainPodcasts.Size = new System.Drawing.Size(66, 20);
            this.menuMainPodcasts.Text = "&Podcasts";
            // 
            // menuMainPodcastsPause
            // 
            this.menuMainPodcastsPause.Image = ((System.Drawing.Image)(resources.GetObject("menuMainPodcastsPause.Image")));
            this.menuMainPodcastsPause.Name = "menuMainPodcastsPause";
            this.menuMainPodcastsPause.Size = new System.Drawing.Size(229, 22);
            this.menuMainPodcastsPause.Text = "&Pause Downloads";
            this.menuMainPodcastsPause.Click += new System.EventHandler(this.PauseClick);
            // 
            // menuMainPodcastsResume
            // 
            this.menuMainPodcastsResume.Image = ((System.Drawing.Image)(resources.GetObject("menuMainPodcastsResume.Image")));
            this.menuMainPodcastsResume.Name = "menuMainPodcastsResume";
            this.menuMainPodcastsResume.Size = new System.Drawing.Size(229, 22);
            this.menuMainPodcastsResume.Text = "&Resume Downlads";
            this.menuMainPodcastsResume.Click += new System.EventHandler(this.ResumeClick);
            // 
            // menuMainPodcastsRefresh
            // 
            this.menuMainPodcastsRefresh.Image = ((System.Drawing.Image)(resources.GetObject("menuMainPodcastsRefresh.Image")));
            this.menuMainPodcastsRefresh.Name = "menuMainPodcastsRefresh";
            this.menuMainPodcastsRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuMainPodcastsRefresh.Size = new System.Drawing.Size(229, 22);
            this.menuMainPodcastsRefresh.Text = "&Check for New Items";
            this.menuMainPodcastsRefresh.Click += new System.EventHandler(this.RefreshAllClick);
            // 
            // menuMainPodcastsOpenFolder
            // 
            this.menuMainPodcastsOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("menuMainPodcastsOpenFolder.Image")));
            this.menuMainPodcastsOpenFolder.Name = "menuMainPodcastsOpenFolder";
            this.menuMainPodcastsOpenFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.menuMainPodcastsOpenFolder.Size = new System.Drawing.Size(229, 22);
            this.menuMainPodcastsOpenFolder.Text = "&Open Podcasts Folder";
            this.menuMainPodcastsOpenFolder.Click += new System.EventHandler(this.OpenPodcastsFolderClick);
            // 
            // menuMainHelp
            // 
            this.menuMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainHelpUserGuide,
            this.menuMainHelpVisitWebsite,
            this.reportABugToolStripMenuItem,
            this.menuMainHelpAbout});
            this.menuMainHelp.Name = "menuMainHelp";
            this.menuMainHelp.Size = new System.Drawing.Size(44, 20);
            this.menuMainHelp.Text = "&Help";
            // 
            // menuMainHelpUserGuide
            // 
            this.menuMainHelpUserGuide.Name = "menuMainHelpUserGuide";
            this.menuMainHelpUserGuide.Size = new System.Drawing.Size(142, 22);
            this.menuMainHelpUserGuide.Text = "&User Guide";
            this.menuMainHelpUserGuide.Click += new System.EventHandler(this.menuMainHelpUserGuideClick);
            // 
            // menuMainHelpVisitWebsite
            // 
            this.menuMainHelpVisitWebsite.Name = "menuMainHelpVisitWebsite";
            this.menuMainHelpVisitWebsite.Size = new System.Drawing.Size(142, 22);
            this.menuMainHelpVisitWebsite.Text = "&Visit Website";
            this.menuMainHelpVisitWebsite.Click += new System.EventHandler(this.menuMainHelpVisitWebsiteClick);
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
            this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.reportABugToolStripMenuItem.Text = "Report a Bug";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuIte_Click);
            // 
            // menuMainHelpAbout
            // 
            this.menuMainHelpAbout.Name = "menuMainHelpAbout";
            this.menuMainHelpAbout.Size = new System.Drawing.Size(142, 22);
            this.menuMainHelpAbout.Text = "&About...";
            this.menuMainHelpAbout.Click += new System.EventHandler(this.menuMainHelpAboutClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(717, 417);
            this.Controls.Add(this.toolStripContainer2);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(488, 264);
            this.Name = "MainForm";
            this.Text = "PodPuppy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Shown += new System.EventHandler(this.MainFormLoad);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuFeed.ResumeLayout(false);
            this.menuIcon.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.menuItem.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private PodPuppy.ListViewNoFlicker listFeeds;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ContextMenuStrip menuFeed;
        private System.Windows.Forms.ToolStripMenuItem menuFeedRemove;
        private System.Windows.Forms.ToolStripMenuItem menuFeedOpenFolder;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip menuIcon;
        private System.Windows.Forms.ToolStripMenuItem menuIconPause;
        private System.Windows.Forms.ToolStripMenuItem menuIconResume;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuIconExit;
        private System.Windows.Forms.ToolStripMenuItem menuIconOpenFolder;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnResumeDownloads;
        private System.Windows.Forms.ToolStripButton btnPauseDownloads;
        private System.Windows.Forms.ToolStripButton btnRemoveSelectedFeeds;
        private System.Windows.Forms.ToolStripButton btnRefreshAll;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripMenuItem menuFeedRefresh;
        private System.Windows.Forms.ToolStripButton btnOpenPodcastsFolder;
        private System.Windows.Forms.ToolStripButton btnRaisePriority;
        private System.Windows.Forms.ToolStripButton btnLowerPriority;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuMainPodcasts;
        private System.Windows.Forms.ToolStripMenuItem menuMainPodcastsPause;
        private System.Windows.Forms.ToolStripMenuItem menuMainPodcastsResume;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelp;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelpVisitWebsite;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileOptions;
        private System.Windows.Forms.ToolStripMenuItem menuIconRefresh;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.Splitter splitter1;
        private PodPuppy.ListViewNoFlicker listItems;
        private System.Windows.Forms.ColumnHeader columnHeaderItemTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderItemStatus;
        private System.Windows.Forms.ContextMenuStrip menuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemPlay;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem menuMainPodcastsRefresh;
        private System.Windows.Forms.ToolStripMenuItem menuMainPodcastsOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyUrl;
        private System.Windows.Forms.ToolStripMenuItem menuFeedCopyUrl;
        private System.Windows.Forms.ToolStripMenuItem menuItemTryAgain;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelpUserGuide;
        private System.Windows.Forms.ToolStripMenuItem menuFeedPlay;
        private System.Windows.Forms.ColumnHeader columnHeaderItemPubDate;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileImport;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewInfo;
        private System.Windows.Forms.ToolStripMenuItem menuMainFileSyncronise;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripProgressBar syncProgressBar;
        private System.Windows.Forms.ColumnHeader columnHeaderItemDownloadDate;
        private System.Windows.Forms.ToolStripMenuItem menuItemSkipSelected;
        private System.Windows.Forms.ToolStripMenuItem menuItemUnskipSelected;
        private System.Windows.Forms.ColumnHeader columnHeaderSynced;
        private System.Windows.Forms.ToolStripStatusLabel lnkCancelSync;
        private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem menuItemTags;
        private System.Windows.Forms.ColumnHeader columnHeaderPriority;
        private System.Windows.Forms.ColumnHeader columnHeaderLatest;
        private System.Windows.Forms.ToolStripMenuItem menuFeedProperties;
    }
}

