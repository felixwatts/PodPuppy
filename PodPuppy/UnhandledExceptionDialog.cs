using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PodPuppy
{
    public partial class UnhandledExceptionDialog : ReportBugDlg
    {
        public UnhandledExceptionDialog(Exception ex)
        {
            InitializeComponent();

            _lblPrompt.Text = "PodPuppy has encountered an unexpected problem and must close. An error report has been generated, please click the Submit button below to send the report to the PodPuppy team along with some diagnostic information.";

            _txtDescription.Text = string.Format("An unhandled exception occurred: {0}\n\n{1}", ex.Message, ex.StackTrace);
        }
    }
}