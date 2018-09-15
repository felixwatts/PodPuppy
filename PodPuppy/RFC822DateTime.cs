using System;
using System.Collections.Generic;
using System.Text;

namespace PodPuppy
{
    class RFC822DateTime
    {
        static public bool TryParse(string date, out DateTime result)
        {
            int pos = date.LastIndexOf(" ");

            if (DateTime.TryParse(date, out result))
                return true;

            string timezone = date.Substring(pos + 1);
            date = date.Substring(0, pos);

            if (!DateTime.TryParse(date, out result))
                return false;

            switch (timezone)
            {
                case "A":
                    result = result.AddHours(1);
                    break;

                case "B":
                    result = result.AddHours(2);
                    break;

                case "C":
                    result = result.AddHours(3);
                    break;

                case "D":
                    result = result.AddHours(4);
                    break;

                case "E":
                    result = result.AddHours(5);
                    break;

                case "F":
                    result = result.AddHours(6);
                    break;

                case "G":
                    result = result.AddHours(7);
                    break;

                case "H":
                    result = result.AddHours(8);
                    break;

                case "I":
                    result = result.AddHours(9);
                    break;

                case "K":
                    result = result.AddHours(10);
                    break;

                case "L":
                    result = result.AddHours(11);
                    break;

                case "M":
                    result = result.AddHours(12);
                    break;

                case "N":
                    result = result.AddHours(-1);
                    break;

                case "O":
                    result = result.AddHours(-2);
                    break;

                case "P":
                    result = result.AddHours(-3);
                    break;

                case "Q":
                    result = result.AddHours(-4);
                    break;

                case "R":
                    result = result.AddHours(-5);
                    break;

                case "S":
                    result = result.AddHours(-6);
                    break;
                case "T":
                    result = result.AddHours(-7);
                    break;

                case "U":
                    result = result.AddHours(-8);
                    break;

                case "V":
                    result = result.AddHours(-9);
                    break;

                case "W":
                    result = result.AddHours(-10);
                    break;

                case "X":
                    result = result.AddHours(-11);
                    break;

                case "Y":
                    result = result.AddHours(-12);
                    break;

                case "EST":
                    result = result.AddHours(5);
                    break;

                case "EDT":
                    result = result.AddHours(4);
                    break;

                case "CST":
                    result = result.AddHours(6);
                    break;

                case "CDT":
                    result = result.AddHours(5);
                    break;

                case "MST":
                    result = result.AddHours(7);
                    break;

                case "MDT":
                    result = result.AddHours(6);
                    break;

                case "PST":
                    result = result.AddHours(8);
                    break;

                case "PDT":
                    result = result.AddHours(7);
                    break;
            }

            return true;

        }
    }
}
