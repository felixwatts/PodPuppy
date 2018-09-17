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
    public partial class FeedTagsDlg : CentredDialog
    {
        private Feed _feed;

        public FeedTagsDlg(Feed feed)
        {
            InitializeComponent();

            _feed = feed;
            feedTagsControl.Populate(feed);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            feedTagsControl.Apply(_feed);
        }
    }
}