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
    public partial class HTMLViewDlg : CentredDialog
    {
        public HTMLViewDlg(string content)
        {
            InitializeComponent();

            webBrowser.DocumentText = content;
        }

        public HTMLViewDlg(Item item) : this(item.Description == null ? "There is no extra information about this item." : item.Description)
        {
        }

        private void webBrowser_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
        {
            try
            {
                if (e.Url.AbsoluteUri != "about:blank")
                {
                    System.Diagnostics.Process.Start(e.Url.AbsoluteUri);
                    e.Cancel = true;
                }
            }
            catch { }
        }
    }
}