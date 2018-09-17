// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace PodPuppy
{
    static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //bool a = Tools.IsValidPath(@"c:\myPath");
            //bool b = Tools.IsValidPath(@"c:\myPath\mypath2");
            //bool c = Tools.IsValidPath(@"c:\myPath\my\path");
            //bool d = Tools.IsValidPath(@"\\server\folder");
            //bool e = Tools.IsValidPath(@"c:\");
            //bool f = Tools.IsValidPath(@"c:\myPath\my\path\");
            //bool g = Tools.IsValidPath(@"c:\myPath_1\my path\path");
            //bool h = Tools.IsValidPath(@"\\server_1\folder\");

            //bool i = Tools.IsValidPath(@"\\server:");
            //bool j = Tools.IsValidPath(@"\\server$");
            //bool k = Tools.IsValidPath(@"c:\myPath:\mypath2");
            //bool l = Tools.IsValidPath(@"c:\myPath\mypath2<");
            //bool l = Tools.IsValidPath(@"c:\myPath\rate my rap");
            
            // only allow one copy to run at a time...
            Process[] processes = Process.GetProcesses();
            foreach (Process proc in processes)
            {
                if (proc.ProcessName == "PodPuppy" && proc.Id != Process.GetCurrentProcess().Id)
                {
                    if (args.Length > 0)
                    {
                        // if started with a url arg then pass to already running instance:
                        InterprocessCommunication.SendString(proc.MainWindowHandle, args[0]);
                    }

                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm(args));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unhandled Exception: " + ex.Message + "\r\n" + ex.StackTrace);
                new UnhandledExceptionDialog(ex).ShowDialog();
            }
        }
    }
}