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
        private string _diagnostics = null;

        public ReportBugDlg()
        {
            InitializeComponent();
        }

        private void CompileDiagnostics()
        {
            if (_diagnostics != null)
                return;

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

                    _diagnostics = sb.ToString();
                }
            }
            if(!foundRightListener)
            {
                Trace.TraceError("Unable to compile diagnostic report, there is no FixedLengthFileTraceListener registered.");
                _diagnostics = "Unable to compile diagnostic report, there is no FixedLengthFileTraceListener registered.";
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            CompileDiagnostics();

            btnSubmit.Text = "Sending...";

            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create("http://fwatts.info/podpuppy/reportBug.php");
            rq.Method = "POST";
            rq.ContentType = "application/x-www-form-urlencoded";
            rq.BeginGetRequestStream(new AsyncCallback(OnGotRequestStream), rq);
        }

        private void OnGotRequestStream(IAsyncResult result)
        {
            try
            {
                HttpWebRequest rq = (HttpWebRequest)result.AsyncState;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(WriteToRequestStream);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.RunWorkerAsync(rq);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while sending bug report: " + ex.Message);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result)
            {
                MessageBox.Show("PodPuppy encountered a problem and was unable to send your bug report. Please contact me at the following address for futher assistance.\n\nfelix@fwatts.info", "PodPuppy Bug Reporting", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Abort;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }

            Close();
        }

        void WriteToRequestStream(object sender, DoWorkEventArgs e)
        {
            try
            {

                WebRequest rq = (WebRequest)e.Argument;
                Stream rqStr = rq.GetRequestStream();
                StreamWriter writer = new StreamWriter(rqStr);
                writer.Write("email=");
                writer.Write(System.Web.HttpUtility.UrlEncode(txtEmail.Text));
                writer.Write("&desc=");
                writer.Write(System.Web.HttpUtility.UrlEncode(_txtDescription.Text));
                writer.Write("&report=");
                writer.Write(System.Web.HttpUtility.UrlEncode(_diagnostics));
                writer.Flush();
                writer.Close();
                e.Result = rq;

                HttpWebResponse rsp = (HttpWebResponse)rq.GetResponse();
                rsp.Close();

                // success
                e.Result = true;
                
                if (rsp.StatusCode != HttpStatusCode.OK)
                {
                    Trace.TraceError("Error while sending bug report: HTTP Status code: " + e.Result.ToString());
                    e.Result = false;
                }                
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while sending bug report: " + ex.Message);
                e.Result = false;
            }
        }

        private void lnkViewDiagnostics_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                CompileDiagnostics();
                string filename = Path.GetTempFileName() + ".txt";
                StreamWriter writer = new StreamWriter(filename);
                writer.Write(_diagnostics);
                writer.Flush();
                writer.Close();
                Process.Start(filename);
            }
            catch { }
        }
    }
}