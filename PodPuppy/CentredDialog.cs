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
