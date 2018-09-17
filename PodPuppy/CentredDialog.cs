// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public class CentredDialog : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Owner != null)
            {
                Left = Owner.Left + ((Owner.Width - Width) / 2);
                Top = Owner.Top + ((Owner.Height - Height) / 2);
            }
        }
    }
}
