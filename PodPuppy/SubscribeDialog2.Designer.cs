namespace PodPuppy
{
    partial class SubscribeDialog2
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
            this._tabItems = new System.Windows.Forms.TabPage();
            this._listItems = new System.Windows.Forms.ListView();
            this._menuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectLatestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tabControl.SuspendLayout();
            this._tabItems.SuspendLayout();
            this._menuItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabItems);
            this._tabControl.Controls.SetChildIndex(this._tabItems, 0);
            // 
            // _btnSubscribe
            // 
            this._btnSubscribe.Text = "Subscribe";
            // 
            // _tabItems
            // 
            this._tabItems.Controls.Add(this._listItems);
            this._tabItems.Location = new System.Drawing.Point(4, 22);
            this._tabItems.Name = "_tabItems";
            this._tabItems.Padding = new System.Windows.Forms.Padding(3);
            this._tabItems.Size = new System.Drawing.Size(514, 213);
            this._tabItems.TabIndex = 2;
            this._tabItems.Text = "Item Selection";
            this._tabItems.UseVisualStyleBackColor = true;
            // 
            // _listItems
            // 
            this._listItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listItems.Location = new System.Drawing.Point(3, 3);
            this._listItems.Name = "_listItems";
            this._listItems.Size = new System.Drawing.Size(508, 207);
            this._listItems.TabIndex = 0;
            this._listItems.UseCompatibleStateImageBehavior = false;
            // 
            // _menuItems
            // 
            this._menuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.selectLatestToolStripMenuItem});
            this._menuItems.Name = "_menuItems";
            this._menuItems.Size = new System.Drawing.Size(140, 70);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // selectLatestToolStripMenuItem
            // 
            this.selectLatestToolStripMenuItem.Name = "selectLatestToolStripMenuItem";
            this.selectLatestToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.selectLatestToolStripMenuItem.Text = "Select Latest";
            this.selectLatestToolStripMenuItem.Click += new System.EventHandler(this.selectLatestToolStripMenuItem_Click);
            // 
            // SubscribeDialog2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 276);
            this.Name = "SubscribeDialog2";
            this.Text = "Subscribe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SubscribeDialog2_FormClosing);
            this.Controls.SetChildIndex(this._tabControl, 0);
            this.Controls.SetChildIndex(this._btnSubscribe, 0);
            this._tabControl.ResumeLayout(false);
            this._tabItems.ResumeLayout(false);
            this._menuItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage _tabItems;
        private System.Windows.Forms.ListView _listItems;
        private System.Windows.Forms.ContextMenuStrip _menuItems;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectLatestToolStripMenuItem;
    }
}