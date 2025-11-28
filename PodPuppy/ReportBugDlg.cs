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
using System.Diagnostics;
using System.IO;
using System.Net;

namespace PodPuppy
{
    public partial class ReportBugDlg : CentredDialog
    {
        public ReportBugDlg()
        {
            InitializeComponent();

            CompileDiagnostics();
        }

        private void CompileDiagnostics()
        {
            StringBuilder sb = new StringBuilder();

            bool foundRightListener = false;
            foreach(TraceListener listener in Trace.Listeners)
            {
                if(listener is FixedLengthFileTraceListener)
                {
                    foundRightListener = true;

                    string errorLog = (listener as FixedLengthFileTraceListener).CurrentContent;

                    sb.Append("Error Log\r\n=========\r\n\r\n");
                    sb.Append(errorLog);

                    sb.Append("\r\n\r\nState File\r\n=========\r\n\r\n");
                    try
                    {
                        string stateFile = Statics.Config.SettingsDir + "\\PodPuppy.state";
                        if(File.Exists(stateFile))
                        {
                            StreamReader reader = new StreamReader(stateFile);
                            sb.Append(reader.ReadToEnd());
                        }
                        else
                        {
                            sb.Append("Does not exist.");
                        }
                    }
                    catch(Exception exception)
                    {
                        sb.Append("Exception when reading: " + exception.Message);
                    }

                    sb.Append("\r\n\r\nConfig File\r\n=========\r\n\r\n");
                    try
                    {
                        string configFile = Statics.Config.ConfigFilename;
                        if(File.Exists(configFile))
                        {
                            StreamReader reader = new StreamReader(configFile);
                            sb.Append(reader.ReadToEnd());
                        }
                        else
                        {
                            sb.Append("Does not exist.");
                        }
                    }
                    catch(Exception exception)
                    {
                        sb.Append("Exception when reading: " + exception.Message);
                    }

                    _txtDiagnosticInformation.Text = sb.ToString();
                }
            }
            if(!foundRightListener)
            {
                Trace.TraceError("Unable to compile diagnostic report, there is no FixedLengthFileTraceListener registered.");
                _txtDiagnosticInformation.Text = "Unable to compile diagnostic report, there is no FixedLengthFileTraceListener registered.";
            }
        }

        private void lnkPodPuppyRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            const string url = "https://github.com/felixwatts/PodPuppy";

            // mark link visited if possible
            var link = sender as LinkLabel;
            if (link != null)
                link.LinkVisited = true;

            try
            {
                // Preferred: let the system open the default browser for the URL
                Process.Start(url);
            }
            catch (Exception)
            {
                // Fallback: use cmd.exe to invoke start (works around some shell issues)
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo("cmd", $"/c start \"\" \"{url}\"")
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the web browser: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}