// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public partial class GetLatestDlg : CentredDialog
    {
        public GetLatestDlg(Feed feed)
        {
            InitializeComponent();

            lblMain.Text = feed.Title + " contains " + feed.Items.Length + " items.";
        }

        private void GetLatestDlg_Load(object sender, EventArgs e)
        {
            int left = Owner.Left + ((Owner.Width - this.Width) / 2);
            int top = Owner.Top + ((Owner.Height - this.Height) / 2);
            this.Location = new Point(left, top);
        }
    }
}