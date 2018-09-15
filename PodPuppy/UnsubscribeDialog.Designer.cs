namespace PodPuppy
{
    partial class UnsubscribeDialog
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
            this._lblQuestion = new System.Windows.Forms.Label();
            this._lblFolder = new System.Windows.Forms.Label();
            this._btnDelete = new System.Windows.Forms.Button();
            this._btnKeep = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lblQuestion
            // 
            this._lblQuestion.Location = new System.Drawing.Point(12, 9);
            this._lblQuestion.Name = "_lblQuestion";
            this._lblQuestion.Size = new System.Drawing.Size(449, 30);
            this._lblQuestion.TabIndex = 0;
            this._lblQuestion.Text = "Do you want to delete the files that are already downloaded? CAUTION: Selecting D" +
                "elete will delete the following folder:";
            // 
            // _lblFolder
            // 
            this._lblFolder.AutoSize = true;
            this._lblFolder.Location = new System.Drawing.Point(12, 49);
            this._lblFolder.Name = "_lblFolder";
            this._lblFolder.Size = new System.Drawing.Size(33, 13);
            this._lblFolder.TabIndex = 0;
            this._lblFolder.Text = "folder";
            // 
            // _btnDelete
            // 
            this._btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnDelete.DialogResult = System.Windows.Forms.DialogResult.No;
            this._btnDelete.Location = new System.Drawing.Point(264, 103);
            this._btnDelete.Name = "_btnDelete";
            this._btnDelete.Size = new System.Drawing.Size(98, 23);
            this._btnDelete.TabIndex = 1;
            this._btnDelete.Text = "Delete Files";
            this._btnDelete.UseVisualStyleBackColor = true;
            // 
            // _btnKeep
            // 
            this._btnKeep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnKeep.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this._btnKeep.Location = new System.Drawing.Point(160, 103);
            this._btnKeep.Name = "_btnKeep";
            this._btnKeep.Size = new System.Drawing.Size(98, 23);
            this._btnKeep.TabIndex = 0;
            this._btnKeep.Text = "Keep Files";
            this._btnKeep.UseVisualStyleBackColor = true;
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(368, 103);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(98, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // UnsubscribeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 138);
            this.ControlBox = false;
            this.Controls.Add(this._btnKeep);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnDelete);
            this.Controls.Add(this._lblFolder);
            this.Controls.Add(this._lblQuestion);
            this.Name = "UnsubscribeDialog";
            this.Text = "Unsubscribe From Podcast";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblQuestion;
        private System.Windows.Forms.Label _lblFolder;
        private System.Windows.Forms.Button _btnDelete;
        private System.Windows.Forms.Button _btnKeep;
        private System.Windows.Forms.Button _btnCancel;
    }
}