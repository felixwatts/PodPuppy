using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public partial class FeedFetchDlg : Form
    {
        private MainForm _mainForm;

        public FeedFetchDlg(MainForm mainForm)
        {
            InitializeComponent();

            _mainForm = mainForm;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _mainForm.CancelFetch();
        }

        private void FeedFetchDlg_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Left = Cursor.Position.X - (Width / 2);
                Top = Cursor.Position.Y - (Height / 2);
            }
        }
    }
}