namespace PodPuppy
{
    partial class ReportBugDlg
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
            this._lblPrompt = new System.Windows.Forms.Label();
            this._txtDiagnosticInformation = new System.Windows.Forms.TextBox();
            this.lnkPodPuppyRepo = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _lblPrompt
            // 
            this._lblPrompt.Location = new System.Drawing.Point(24, 22);
            this._lblPrompt.Name = "_lblPrompt";
            this._lblPrompt.Size = new System.Drawing.Size(149, 16);
            this._lblPrompt.TabIndex = 0;
            this._lblPrompt.Text = "Please report any bugs at the";
            // 
            // _txtDiagnosticInformation
            // 
            this._txtDiagnosticInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._txtDiagnosticInformation.Enabled = false;
            this._txtDiagnosticInformation.Location = new System.Drawing.Point(27, 110);
            this._txtDiagnosticInformation.Multiline = true;
            this._txtDiagnosticInformation.Name = "_txtDiagnosticInformation";
            this._txtDiagnosticInformation.Size = new System.Drawing.Size(364, 168);
            this._txtDiagnosticInformation.TabIndex = 1;
            // 
            // lnkPodPuppyRepo
            // 
            this.lnkPodPuppyRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPodPuppyRepo.AutoSize = true;
            this.lnkPodPuppyRepo.Location = new System.Drawing.Point(166, 22);
            this.lnkPodPuppyRepo.Name = "lnkPodPuppyRepo";
            this.lnkPodPuppyRepo.Size = new System.Drawing.Size(112, 13);
            this.lnkPodPuppyRepo.TabIndex = 3;
            this.lnkPodPuppyRepo.TabStop = true;
            this.lnkPodPuppyRepo.Text = "PodPuppy github repo";
            this.lnkPodPuppyRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPodPuppyRepo_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Diagnostic Information";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(315, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 40);
            this.label1.TabIndex = 8;
            this.label1.Text = "It may be helpful to include the following diagnostic information in your bug rep" +
    "ort";
            // 
            // ReportBugDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 325);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lnkPodPuppyRepo);
            this.Controls.Add(this._txtDiagnosticInformation);
            this.Controls.Add(this._lblPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ReportBugDlg";
            this.Text = "Report a Bug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel lnkPodPuppyRepo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        protected System.Windows.Forms.Label _lblPrompt;
        protected System.Windows.Forms.TextBox _txtDiagnosticInformation;
        protected System.Windows.Forms.Label label1;
    }
}