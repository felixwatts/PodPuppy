using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public partial class UnsubscribeDialog : CentredDialog
    {
        public UnsubscribeDialog(string folder)
        {
            InitializeComponent();

            _lblFolder.Text = folder;
        }
    }
}