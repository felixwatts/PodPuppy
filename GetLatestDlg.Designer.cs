namespace PodPuppy
{
    partial class GetLatestDlg
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
            this.lblMain = new System.Windows.Forms.Label();
            this.btnGetAll = new System.Windows.Forms.Button();
            this.btnGetLatest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Location = new System.Drawing.Point(12, 9);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(179, 13);
            this.lblMain.TabIndex = 0;
            this.lblMain.Text = "Dhamma Podcast contains 17 items.";
            // 
            // btnGetAll
            // 
            this.btnGetAll.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnGetAll.Location = new System.Drawing.Point(72, 47);
            this.btnGetAll.Name = "btnGetAll";
            this.btnGetAll.Size = new System.Drawing.Size(178, 23);
            this.btnGetAll.TabIndex = 1;
            this.btnGetAll.Text = "Get Them All";
            this.btnGetAll.UseVisualStyleBackColor = true;
            // 
            // btnGetLatest
            // 
            this.btnGetLatest.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnGetLatest.Location = new System.Drawing.Point(256, 47);
            this.btnGetLatest.Name = "btnGetLatest";
            this.btnGetLatest.Size = new System.Drawing.Size(178, 23);
            this.btnGetLatest.TabIndex = 2;
            this.btnGetLatest.Text = "Get the Latest";
            this.btnGetLatest.UseVisualStyleBackColor = true;
            // 
            // GetLatestDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 81);
            this.ControlBox = false;
            this.Controls.Add(this.btnGetLatest);
            this.Controls.Add(this.btnGetAll);
            this.Controls.Add(this.lblMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GetLatestDlg";
            this.Text = "Subscribe to Podcast";
            this.Load += new System.EventHandler(this.GetLatestDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMain;
        private System.Windows.Forms.Button btnGetAll;
        private System.Windows.Forms.Button btnGetLatest;
    }
}