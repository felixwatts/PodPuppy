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
            if (!parserRgx.IsMatch(rangeString))
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
}
