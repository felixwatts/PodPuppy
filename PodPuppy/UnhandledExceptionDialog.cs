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
    public partial class UnhandledExceptionDialog : ReportBugDlg
    {
        public UnhandledExceptionDialog(Exception ex)
        {
            InitializeComponent();

            _lblPrompt.Text = "PodPuppy has encountered an unexpected problem and must close. An error report has been generated, please click the Submit button below to send the report to the PodPuppy team along with some diagnostic information.";

            _txtDiagnosticInformation.Text = string.Format("An unhandled exception occurred: {0}\n\n{1}", ex.Message, ex.StackTrace);
        }
    }
}