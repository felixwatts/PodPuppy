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