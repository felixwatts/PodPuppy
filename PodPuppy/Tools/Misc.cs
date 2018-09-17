// PodPuppy - a simple podcast receiver for Windows
// Copyright (c) Felix Watts 2008 (felixwatts@gmail.com)
// https://github.com/felixwatts/PodPuppy
//
// This file is distributed under the Creative Commons Attribution-NonCommercial 4.0 International Licence
// https://creativecommons.org/licenses/by-nc/4.0/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PodPuppy
{
    public class Tools
    {
        private static Regex _rgxHtmlTag;
        private const int MAX_PATH_LENGTH = 248;

        static Tools()
        {
            _rgxHtmlTag = new Regex("<.+?>|&(?<charCode>.+?);", RegexOptions.Compiled);
        }

        public static bool IsValidPath(string path)
        {
            // TODO
            return true;
        }

        public static string ToSensibleTimeString(DateTime? time)
        {
            if (time == null)
                return "";
            else return ToSensibleTimeString(time.Value);
        }

        public static string ToSensibleTimeString(DateTime time)
        {
            if (time == DateTime.MinValue)
                return "";

            string text = "";

            if (time.Date == DateTime.Now.Date)
                text = time.ToShortTimeString();
            else text = time.ToShortDateString() + " " + time.ToShortTimeString();

            return text;
        }

        public static string EnsurePathIsNotTooLong(string path)
        {
            if (path.Length > MAX_PATH_LENGTH)
            {
                int lastSlashIndex = path.LastIndexOf('\\');
                if (lastSlashIndex < 0)
                    lastSlashIndex = 0;

                int lastDotIndex = path.LastIndexOf('.');
                if (lastDotIndex < lastSlashIndex)
                    lastDotIndex = path.Length;

                string ext = path.Substring(lastDotIndex);
                return path.Substring(0, MAX_PATH_LENGTH - ext.Length - 3) + "..." + ext;
            }
            else return path;
        }

        public static void ShowTagHelp()
        {
            try
            {
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + "\\Tags.html");
            }
            catch { }
        }

        public static string StripHTML(string input)
        {
            return _rgxHtmlTag.Replace(input, StripHtmlMatchEvaluator).Trim();
        }

        private static string StripHtmlMatchEvaluator(Match match)
        {
            if (match.Value.StartsWith("<"))
                return "";

            string code = match.Groups["charCode"].Value;

            switch (code)
            {
                case "amp": return "&";
                case "quot": return "\"";
                default:
                    if (code.StartsWith("#"))
                    {
                        int charNum;
                        if (int.TryParse(code.Substring(1), out charNum))
                            return new string(Encoding.Unicode.GetChars(new byte[] { (byte)charNum }));
                        else
                        {
                            System.Diagnostics.Trace.TraceError("Error stripping html. Invalid character code: " + code);
                            return "";
                        }
                    }
                    else
                    {
                        return ""; // TODO
                    }
            }
        }
    }
}
