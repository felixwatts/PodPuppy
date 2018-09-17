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
using System.IO;

namespace PodPuppy
{
    /// <summary>
    /// Used to parse the Content Range entry in an HTTP response header.
    /// </summary>
    class ContentRange
    {
        private long _rangeMin;
        public long RangeMin
        {
            get { return _rangeMin; }
        }

        private long _rangeMax;
        public long RangeMax
        {
            get { return _rangeMax; }
        }

        private long _total;
        public long Total
        {
            get { return _total; }
        }

        private static Regex parserRgx;

        static ContentRange()
        {
            // e.g. "bytes 0-499/1234"
            parserRgx = new Regex(@"bytes (?<rangeMin>\d+)?-(?<rangeMax>\d+)\/(?<total>\d+)");

            // NOTE: changed to accept
            // "bytes -1/10" meaning the final 1 byte.
            // This is not proper http but I have encountered it from servers.
        }

        public static ContentRange Parse(string rangeString)
        {
            if(!parserRgx.IsMatch(rangeString))
                return null;

            Match match = parserRgx.Match(rangeString);

            long max = long.Parse(match.Groups["rangeMax"].Value);
            long total = long.Parse(match.Groups["total"].Value);

            long min;
            if (match.Groups["rangeMin"].Value == "")
            {
                min = total - max;
                max = total;
            }
            else min = long.Parse(match.Groups["rangeMin"].Value);

            return new ContentRange(min, max, total);
        }

        public ContentRange(long min, long max, long total)
        {
            _rangeMax = max;
            _rangeMin = min;
            _total = total;
        }
    }

    public class Tools
    {
        private static Regex _rgxHtmlTag;
        private static Regex _pathRgx;
        private const int MAX_PATH_LENGTH = 248; 

        static Tools()
        {            
            _rgxHtmlTag = new Regex("<.+?>|&(?<charCode>.+?);", RegexOptions.Compiled);

            string invalidPathChars = @"";
            _pathRgx = new Regex(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\([^:<>""?/\\|*]*))*\\?$", RegexOptions.Compiled);
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

        public static bool IsValidPath(string path)
        {
            return _pathRgx.IsMatch(path);
        }
    }
}
