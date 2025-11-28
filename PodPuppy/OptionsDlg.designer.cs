namespace PodPuppy
{
    partial class OptionsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabSync = new System.Windows.Forms.TabPage();
            this.txtSyncedFileTypes = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkAutoSync = new System.Windows.Forms.CheckBox();
            this.btnBrowseSyncFolder = new System.Windows.Forms.Button();
            this.txtSyncFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabScheduler = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scheduleControl = new PodPuppy.ScheduleControl();
            this.chkEnableScheduler = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbBalloonFunction = new System.Windows.Forms.GroupBox();
            this.rbBalloonOpenFolder = new System.Windows.Forms.RadioButton();
            this.rbBalloonPlayFile = new System.Windows.Forms.RadioButton();
            this.chkShowBalloons = new System.Windows.Forms.CheckBox();
            this.tabDownloads = new System.Windows.Forms.TabPage();
            this.cmbCheckIntervalUnits = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLimitBandwidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseCompleteFolder = new System.Windows.Forms.Button();
            this.txtCheckInterval = new System.Windows.Forms.TextBox();
            this.btnBrowseTempFolder = new System.Windows.Forms.Button();
            this.txtMaxDownloads = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDownloadFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompleteFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDynamicOPML = new System.Windows.Forms.TabPage();
            this.chkDynamicOPMLDeleteFiles = new System.Windows.Forms.CheckBox();
            this.chkDynamicOPMLJustGetLatest = new System.Windows.Forms.CheckBox();
            this.txtDynamicSubscriptionSrc = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.chkWritePlaylists = new System.Windows.Forms.CheckBox();
            this.chkIncludeFiletypeInSearch = new System.Windows.Forms.CheckBox();
            this.chkShowInTaskbarWhenMinimized = new System.Windows.Forms.CheckBox();
            this.chkStartMinimised = new System.Windows.Forms.CheckBox();
            this.chkLaunchOnStartup = new System.Windows.Forms.CheckBox();
            this.btnRestoreDefaults = new System.Windows.Forms.Button();
            this.tabSync.SuspendLayout();
            this.tabScheduler.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbBalloonFunction.SuspendLayout();
            this.tabDownloads.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDynamicOPML.SuspendLayout();
            this.tabMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "Cancel";
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(364, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = "The Ok Button";
            this.btnOK.AccessibleName = "OK";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(283, 263);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // tabSync
            // 
            this.tabSync.Controls.Add(this.txtSyncedFileTypes);
            this.tabSync.Controls.Add(this.label11);
            this.tabSync.Controls.Add(this.label7);
            this.tabSync.Controls.Add(this.chkAutoSync);
            this.tabSync.Controls.Add(this.btnBrowseSyncFolder);
            this.tabSync.Controls.Add(this.txtSyncFolder);
            this.tabSync.Controls.Add(this.label4);
            this.tabSync.Location = new System.Drawing.Point(4, 22);
            this.tabSync.Name = "tabSync";
            this.tabSync.Padding = new System.Windows.Forms.Padding(3);
            this.tabSync.Size = new System.Drawing.Size(420, 211);
            this.tabSync.TabIndex = 4;
            this.tabSync.Text = "Syncroniser";
            this.tabSync.UseVisualStyleBackColor = true;
            // 
            // txtSyncedFileTypes
            // 
            this.txtSyncedFileTypes.Location = new System.Drawing.Point(118, 151);
            this.txtSyncedFileTypes.MaxLength = 1024;
            this.txtSyncedFileTypes.Name = "txtSyncedFileTypes";
            this.txtSyncedFileTypes.Size = new System.Drawing.Size(284, 20);
            this.txtSyncedFileTypes.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 154);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Synced file types";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(18, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(384, 42);
            this.label7.TabIndex = 12;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // chkAutoSync
            // 
            this.chkAutoSync.AutoSize = true;
            this.chkAutoSync.Location = new System.Drawing.Point(21, 119);
            this.chkAutoSync.Name = "chkAutoSync";
            this.chkAutoSync.Size = new System.Drawing.Size(321, 17);
            this.chkAutoSync.TabIndex = 1;
            this.chkAutoSync.Text = "Automatically syncronise when the removable drive is detected";
            this.chkAutoSync.UseVisualStyleBackColor = true;
            // 
            // btnBrowseSyncFolder
            // 
            this.btnBrowseSyncFolder.Image = global::PodPuppy.Properties.Resources.IconFolder;
            this.btnBrowseSyncFolder.Location = new System.Drawing.Point(379, 80);
            this.btnBrowseSyncFolder.Name = "btnBrowseSyncFolder";
            this.btnBrowseSyncFolder.Size = new System.Drawing.Size(23, 23);
            this.btnBrowseSyncFolder.TabIndex = 0;
            this.btnBrowseSyncFolder.UseVisualStyleBackColor = true;
            this.btnBrowseSyncFolder.Click += new System.EventHandler(this.btnBrowseSyncFolder_Click);
            // 
            // txtSyncFolder
            // 
            this.txtSyncFolder.Location = new System.Drawing.Point(118, 82);
            this.txtSyncFolder.MaxLength = 1024;
            this.txtSyncFolder.Name = "txtSyncFolder";
            this.txtSyncFolder.ReadOnly = true;
            this.txtSyncFolder.Size = new System.Drawing.Size(255, 20);
            this.txtSyncFolder.TabIndex = 0;
            this.txtSyncFolder.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Syncronised folder";
            // 
            // tabScheduler
            // 
            this.tabScheduler.Controls.Add(this.panel2);
            this.tabScheduler.Controls.Add(this.chkEnableScheduler);
            this.tabScheduler.Location = new System.Drawing.Point(4, 22);
            this.tabScheduler.Name = "tabScheduler";
            this.tabScheduler.Padding = new System.Windows.Forms.Padding(3);
            this.tabScheduler.Size = new System.Drawing.Size(420, 211);
            this.tabScheduler.TabIndex = 3;
            this.tabScheduler.Text = "Scheduler";
            this.tabScheduler.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::PodPuppy.Properties.Resources.scheduleControlBg;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.scheduleControl);
            this.panel2.Location = new System.Drawing.Point(6, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(388, 162);
            this.panel2.TabIndex = 1;
            // 
            // scheduleControl
            // 
            this.scheduleControl.Data = null;
            this.scheduleControl.Location = new System.Drawing.Point(69, 39);
            this.scheduleControl.Name = "scheduleControl";
            this.scheduleControl.Size = new System.Drawing.Size(319, 98);
            this.scheduleControl.TabIndex = 0;
            this.scheduleControl.Text = "scheduleControl1";
            // 
            // chkEnableScheduler
            // 
            this.chkEnableScheduler.AutoSize = true;
            this.chkEnableScheduler.Location = new System.Drawing.Point(19, 20);
            this.chkEnableScheduler.Name = "chkEnableScheduler";
            this.chkEnableScheduler.Size = new System.Drawing.Size(108, 17);
            this.chkEnableScheduler.TabIndex = 0;
            this.chkEnableScheduler.Text = "Enable scheduler";
            this.chkEnableScheduler.UseVisualStyleBackColor = true;
            this.chkEnableScheduler.CheckedChanged += new System.EventHandler(this.chkEnableScheduler_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbBalloonFunction);
            this.tabPage2.Controls.Add(this.chkShowBalloons);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(420, 211);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Balloons";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbBalloonFunction
            // 
            this.gbBalloonFunction.Controls.Add(this.rbBalloonOpenFolder);
            this.gbBalloonFunction.Controls.Add(this.rbBalloonPlayFile);
            this.gbBalloonFunction.Location = new System.Drawing.Point(19, 43);
            this.gbBalloonFunction.Name = "gbBalloonFunction";
            this.gbBalloonFunction.Size = new System.Drawing.Size(235, 65);
            this.gbBalloonFunction.TabIndex = 1;
            this.gbBalloonFunction.TabStop = false;
            this.gbBalloonFunction.Text = "Click on the balloon to...";
            // 
            // rbBalloonOpenFolder
            // 
            this.rbBalloonOpenFolder.AutoSize = true;
            this.rbBalloonOpenFolder.Location = new System.Drawing.Point(6, 42);
            this.rbBalloonOpenFolder.Name = "rbBalloonOpenFolder";
            this.rbBalloonOpenFolder.Size = new System.Drawing.Size(150, 17);
            this.rbBalloonOpenFolder.TabIndex = 1;
            this.rbBalloonOpenFolder.Text = "Open the containing folder";
            this.rbBalloonOpenFolder.UseVisualStyleBackColor = true;
            // 
            // rbBalloonPlayFile
            // 
            this.rbBalloonPlayFile.AutoSize = true;
            this.rbBalloonPlayFile.Checked = true;
            this.rbBalloonPlayFile.Location = new System.Drawing.Point(6, 19);
            this.rbBalloonPlayFile.Name = "rbBalloonPlayFile";
            this.rbBalloonPlayFile.Size = new System.Drawing.Size(79, 17);
            this.rbBalloonPlayFile.TabIndex = 0;
            this.rbBalloonPlayFile.TabStop = true;
            this.rbBalloonPlayFile.Text = "Play the file";
            this.rbBalloonPlayFile.UseVisualStyleBackColor = true;
            // 
            // chkShowBalloons
            // 
            this.chkShowBalloons.AutoSize = true;
            this.chkShowBalloons.Checked = true;
            this.chkShowBalloons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowBalloons.Location = new System.Drawing.Point(19, 20);
            this.chkShowBalloons.Name = "chkShowBalloons";
            this.chkShowBalloons.Size = new System.Drawing.Size(235, 17);
            this.chkShowBalloons.TabIndex = 0;
            this.chkShowBalloons.Text = "Pop up a balloon when download completes";
            this.chkShowBalloons.UseVisualStyleBackColor = true;
            this.chkShowBalloons.CheckedChanged += new System.EventHandler(this.chkShowBalloons_CheckedChanged);
            // 
            // tabDownloads
            // 
            this.tabDownloads.Controls.Add(this.cmbCheckIntervalUnits);
            this.tabDownloads.Controls.Add(this.label10);
            this.tabDownloads.Controls.Add(this.label9);
            this.tabDownloads.Controls.Add(this.txtLimitBandwidth);
            this.tabDownloads.Controls.Add(this.label1);
            this.tabDownloads.Controls.Add(this.btnBrowseCompleteFolder);
            this.tabDownloads.Controls.Add(this.txtCheckInterval);
            this.tabDownloads.Controls.Add(this.btnBrowseTempFolder);
            this.tabDownloads.Controls.Add(this.txtMaxDownloads);
            this.tabDownloads.Controls.Add(this.label5);
            this.tabDownloads.Controls.Add(this.txtDownloadFolder);
            this.tabDownloads.Controls.Add(this.label2);
            this.tabDownloads.Controls.Add(this.txtCompleteFolder);
            this.tabDownloads.Controls.Add(this.label3);
            this.tabDownloads.Location = new System.Drawing.Point(4, 22);
            this.tabDownloads.Name = "tabDownloads";
            this.tabDownloads.Padding = new System.Windows.Forms.Padding(3);
            this.tabDownloads.Size = new System.Drawing.Size(420, 211);
            this.tabDownloads.TabIndex = 0;
            this.tabDownloads.Text = "Downloads";
            this.tabDownloads.UseVisualStyleBackColor = true;
            // 
            // cmbCheckIntervalUnits
            // 
            this.cmbCheckIntervalUnits.FormattingEnabled = true;
            this.cmbCheckIntervalUnits.Items.AddRange(new object[] {
            "Hours",
            "Minutes"});
            this.cmbCheckIntervalUnits.Location = new System.Drawing.Point(240, 8);
            this.cmbCheckIntervalUnits.Name = "cmbCheckIntervalUnits";
            this.cmbCheckIntervalUnits.Size = new System.Drawing.Size(74, 21);
            this.cmbCheckIntervalUnits.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(240, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "KB/s (0 = Unlimited)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Maximum Total Download Speed";
            // 
            // txtLimitBandwidth
            // 
            this.txtLimitBandwidth.Location = new System.Drawing.Point(189, 60);
            this.txtLimitBandwidth.MaxLength = 4;
            this.txtLimitBandwidth.Name = "txtLimitBandwidth";
            this.txtLimitBandwidth.Size = new System.Drawing.Size(45, 20);
            this.txtLimitBandwidth.TabIndex = 3;
            this.txtLimitBandwidth.Validating += new System.ComponentModel.CancelEventHandler(this.txtLimitBandwidth_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Maximum Simultaneous Downloads";
            // 
            // btnBrowseCompleteFolder
            // 
            this.btnBrowseCompleteFolder.Image = global::PodPuppy.Properties.Resources.IconFolder;
            this.btnBrowseCompleteFolder.Location = new System.Drawing.Point(391, 112);
            this.btnBrowseCompleteFolder.Name = "btnBrowseCompleteFolder";
            this.btnBrowseCompleteFolder.Size = new System.Drawing.Size(23, 23);
            this.btnBrowseCompleteFolder.TabIndex = 5;
            this.btnBrowseCompleteFolder.UseVisualStyleBackColor = true;
            this.btnBrowseCompleteFolder.Click += new System.EventHandler(this.btnBrowseCompleteFolder_Click);
            // 
            // txtCheckInterval
            // 
            this.txtCheckInterval.Location = new System.Drawing.Point(189, 8);
            this.txtCheckInterval.MaxLength = 2;
            this.txtCheckInterval.Name = "txtCheckInterval";
            this.txtCheckInterval.Size = new System.Drawing.Size(45, 20);
            this.txtCheckInterval.TabIndex = 0;
            this.txtCheckInterval.Validating += new System.ComponentModel.CancelEventHandler(this.txtCheckInterval_Validating);
            // 
            // btnBrowseTempFolder
            // 
            this.btnBrowseTempFolder.Image = global::PodPuppy.Properties.Resources.IconFolder;
            this.btnBrowseTempFolder.Location = new System.Drawing.Point(391, 84);
            this.btnBrowseTempFolder.Name = "btnBrowseTempFolder";
            this.btnBrowseTempFolder.Size = new System.Drawing.Size(23, 23);
            this.btnBrowseTempFolder.TabIndex = 4;
            this.btnBrowseTempFolder.UseVisualStyleBackColor = true;
            this.btnBrowseTempFolder.Click += new System.EventHandler(this.btnBrowseTempFolder_Click);
            // 
            // txtMaxDownloads
            // 
            this.txtMaxDownloads.Location = new System.Drawing.Point(189, 34);
            this.txtMaxDownloads.MaxLength = 2;
            this.txtMaxDownloads.Name = "txtMaxDownloads";
            this.txtMaxDownloads.Size = new System.Drawing.Size(45, 20);
            this.txtMaxDownloads.TabIndex = 2;
            this.txtMaxDownloads.Validating += new System.ComponentModel.CancelEventHandler(this.txtMaxDownloads_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Check Feeds Every";
            // 
            // txtDownloadFolder
            // 
            this.txtDownloadFolder.Location = new System.Drawing.Point(189, 86);
            this.txtDownloadFolder.MaxLength = 1024;
            this.txtDownloadFolder.Name = "txtDownloadFolder";
            this.txtDownloadFolder.Size = new System.Drawing.Size(196, 20);
            this.txtDownloadFolder.TabIndex = 4;
            this.txtDownloadFolder.TabStop = false;
            this.txtDownloadFolder.Validating += new System.ComponentModel.CancelEventHandler(this.txtDownloadFolder_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Temporary Download Folder";
            // 
            // txtCompleteFolder
            // 
            this.txtCompleteFolder.Location = new System.Drawing.Point(189, 114);
            this.txtCompleteFolder.MaxLength = 1024;
            this.txtCompleteFolder.Name = "txtCompleteFolder";
            this.txtCompleteFolder.Size = new System.Drawing.Size(196, 20);
            this.txtCompleteFolder.TabIndex = 5;
            this.txtCompleteFolder.TabStop = false;
            this.txtCompleteFolder.Validating += new System.ComponentModel.CancelEventHandler(this.txtCompleteFolder_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Podcasts Folder";
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleName = "Options Tabs";
            this.tabControl1.Controls.Add(this.tabDownloads);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabScheduler);
            this.tabControl1.Controls.Add(this.tabSync);
            this.tabControl1.Controls.Add(this.tabDynamicOPML);
            this.tabControl1.Controls.Add(this.tabMisc);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(428, 237);
            this.tabControl1.TabIndex = 0;
            // 
            // tabDynamicOPML
            // 
            this.tabDynamicOPML.Controls.Add(this.chkDynamicOPMLDeleteFiles);
            this.tabDynamicOPML.Controls.Add(this.chkDynamicOPMLJustGetLatest);
            this.tabDynamicOPML.Controls.Add(this.txtDynamicSubscriptionSrc);
            this.tabDynamicOPML.Controls.Add(this.label12);
            this.tabDynamicOPML.Controls.Add(this.label13);
            this.tabDynamicOPML.Location = new System.Drawing.Point(4, 22);
            this.tabDynamicOPML.Name = "tabDynamicOPML";
            this.tabDynamicOPML.Padding = new System.Windows.Forms.Padding(3);
            this.tabDynamicOPML.Size = new System.Drawing.Size(420, 211);
            this.tabDynamicOPML.TabIndex = 6;
            this.tabDynamicOPML.Text = "Dynamic OPML";
            this.tabDynamicOPML.UseVisualStyleBackColor = true;
            // 
            // chkDynamicOPMLDeleteFiles
            // 
            this.chkDynamicOPMLDeleteFiles.AutoSize = true;
            this.chkDynamicOPMLDeleteFiles.Location = new System.Drawing.Point(22, 153);
            this.chkDynamicOPMLDeleteFiles.Name = "chkDynamicOPMLDeleteFiles";
            this.chkDynamicOPMLDeleteFiles.Size = new System.Drawing.Size(246, 17);
            this.chkDynamicOPMLDeleteFiles.TabIndex = 2;
            this.chkDynamicOPMLDeleteFiles.Text = "When unsubscribing, delete downloaded items";
            this.chkDynamicOPMLDeleteFiles.UseVisualStyleBackColor = true;
            // 
            // chkDynamicOPMLJustGetLatest
            // 
            this.chkDynamicOPMLJustGetLatest.AutoSize = true;
            this.chkDynamicOPMLJustGetLatest.Location = new System.Drawing.Point(22, 121);
            this.chkDynamicOPMLJustGetLatest.Name = "chkDynamicOPMLJustGetLatest";
            this.chkDynamicOPMLJustGetLatest.Size = new System.Drawing.Size(219, 17);
            this.chkDynamicOPMLJustGetLatest.TabIndex = 1;
            this.chkDynamicOPMLJustGetLatest.Text = "When subscribing, just get the latest item";
            this.chkDynamicOPMLJustGetLatest.UseVisualStyleBackColor = true;
            // 
            // txtDynamicSubscriptionSrc
            // 
            this.txtDynamicSubscriptionSrc.Location = new System.Drawing.Point(143, 90);
            this.txtDynamicSubscriptionSrc.MaxLength = 1024;
            this.txtDynamicSubscriptionSrc.Name = "txtDynamicSubscriptionSrc";
            this.txtDynamicSubscriptionSrc.Size = new System.Drawing.Size(251, 20);
            this.txtDynamicSubscriptionSrc.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(118, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Dynamic OPML Source";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(19, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(381, 60);
            this.label13.TabIndex = 0;
            this.label13.Text = resources.GetString("label13.Text");
            // 
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.chkWritePlaylists);
            this.tabMisc.Controls.Add(this.chkIncludeFiletypeInSearch);
            this.tabMisc.Controls.Add(this.chkShowInTaskbarWhenMinimized);
            this.tabMisc.Controls.Add(this.chkStartMinimised);
            this.tabMisc.Controls.Add(this.chkLaunchOnStartup);
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabMisc.Size = new System.Drawing.Size(420, 211);
            this.tabMisc.TabIndex = 7;
            this.tabMisc.Text = "Misc.";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // chkWritePlaylists
            // 
            this.chkWritePlaylists.AutoSize = true;
            this.chkWritePlaylists.Location = new System.Drawing.Point(16, 112);
            this.chkWritePlaylists.Name = "chkWritePlaylists";
            this.chkWritePlaylists.Size = new System.Drawing.Size(90, 17);
            this.chkWritePlaylists.TabIndex = 5;
            this.chkWritePlaylists.Text = "Write playlists";
            this.chkWritePlaylists.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFiletypeInSearch
            // 
            this.chkIncludeFiletypeInSearch.AutoSize = true;
            this.chkIncludeFiletypeInSearch.Location = new System.Drawing.Point(16, 89);
            this.chkIncludeFiletypeInSearch.Name = "chkIncludeFiletypeInSearch";
            this.chkIncludeFiletypeInSearch.Size = new System.Drawing.Size(146, 17);
            this.chkIncludeFiletypeInSearch.TabIndex = 3;
            this.chkIncludeFiletypeInSearch.Text = "Include file type in search";
            this.chkIncludeFiletypeInSearch.UseVisualStyleBackColor = true;
            // 
            // chkShowInTaskbarWhenMinimized
            // 
            this.chkShowInTaskbarWhenMinimized.AutoSize = true;
            this.chkShowInTaskbarWhenMinimized.Location = new System.Drawing.Point(16, 66);
            this.chkShowInTaskbarWhenMinimized.Name = "chkShowInTaskbarWhenMinimized";
            this.chkShowInTaskbarWhenMinimized.Size = new System.Drawing.Size(179, 17);
            this.chkShowInTaskbarWhenMinimized.TabIndex = 2;
            this.chkShowInTaskbarWhenMinimized.Text = "Show in taskbar when minimized";
            this.chkShowInTaskbarWhenMinimized.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimised
            // 
            this.chkStartMinimised.AutoSize = true;
            this.chkStartMinimised.Location = new System.Drawing.Point(16, 43);
            this.chkStartMinimised.Name = "chkStartMinimised";
            this.chkStartMinimised.Size = new System.Drawing.Size(96, 17);
            this.chkStartMinimised.TabIndex = 1;
            this.chkStartMinimised.Text = "Start minimised";
            this.chkStartMinimised.UseVisualStyleBackColor = true;
            // 
            // chkLaunchOnStartup
            // 
            this.chkLaunchOnStartup.AutoSize = true;
            this.chkLaunchOnStartup.Location = new System.Drawing.Point(16, 20);
            this.chkLaunchOnStartup.Name = "chkLaunchOnStartup";
            this.chkLaunchOnStartup.Size = new System.Drawing.Size(218, 17);
            this.chkLaunchOnStartup.TabIndex = 0;
            this.chkLaunchOnStartup.Text = "Launch PodPuppy when Windows starts";
            this.chkLaunchOnStartup.UseVisualStyleBackColor = true;
            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.AccessibleName = "Restore Defaults";
            this.btnRestoreDefaults.Location = new System.Drawing.Point(12, 263);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.Size = new System.Drawing.Size(163, 23);
            this.btnRestoreDefaults.TabIndex = 1;
            this.btnRestoreDefaults.Text = "Restore Defaults";
            this.btnRestoreDefaults.UseVisualStyleBackColor = true;
            this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);
            // 
            // OptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(451, 298);
            this.ControlBox = false;
            this.Controls.Add(this.btnRestoreDefaults);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionsDlg";
            this.Text = "PodPuppy Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsDlg_FormClosing);
            this.Load += new System.EventHandler(this.OptionsDlg_Load);
            this.tabSync.ResumeLayout(false);
            this.tabSync.PerformLayout();
            this.tabScheduler.ResumeLayout(false);
            this.tabScheduler.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.gbBalloonFunction.ResumeLayout(false);
            this.gbBalloonFunction.PerformLayout();
            this.tabDownloads.ResumeLayout(false);
            this.tabDownloads.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabDynamicOPML.ResumeLayout(false);
            this.tabDynamicOPML.PerformLayout();
            this.tabMisc.ResumeLayout(false);
            this.tabMisc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TabPage tabSync;
        private System.Windows.Forms.TabPage tabScheduler;
        private System.Windows.Forms.Panel panel2;
        private ScheduleControl scheduleControl;
        private System.Windows.Forms.CheckBox chkEnableScheduler;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbBalloonFunction;
        private System.Windows.Forms.RadioButton rbBalloonOpenFolder;
        private System.Windows.Forms.RadioButton rbBalloonPlayFile;
        private System.Windows.Forms.CheckBox chkShowBalloons;
        private System.Windows.Forms.TabPage tabDownloads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseCompleteFolder;
        private System.Windows.Forms.TextBox txtCheckInterval;
        private System.Windows.Forms.Button btnBrowseTempFolder;
        private System.Windows.Forms.TextBox txtMaxDownloads;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDownloadFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompleteFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnBrowseSyncFolder;
        private System.Windows.Forms.TextBox txtSyncFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkAutoSync;
        private System.Windows.Forms.TabPage tabDynamicOPML;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLimitBandwidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSyncedFileTypes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabMisc;
        private System.Windows.Forms.CheckBox chkIncludeFiletypeInSearch;
        private System.Windows.Forms.CheckBox chkShowInTaskbarWhenMinimized;
        private System.Windows.Forms.CheckBox chkStartMinimised;
        private System.Windows.Forms.CheckBox chkLaunchOnStartup;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtDynamicSubscriptionSrc;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkDynamicOPMLJustGetLatest;
        private System.Windows.Forms.CheckBox chkDynamicOPMLDeleteFiles;
        private System.Windows.Forms.Button btnRestoreDefaults;
        private System.Windows.Forms.ComboBox cmbCheckIntervalUnits;
        private System.Windows.Forms.CheckBox chkWritePlaylists;
    }
}
