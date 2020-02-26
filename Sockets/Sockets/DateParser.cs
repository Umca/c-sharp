using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Sockets
{
    static class DateParser
    {
        public static Regex regex = new Regex(@"\b(?<one>\d{1,4})(-|\/|.| )(?<two>(\d{1,2}|\w+))(-|\/|.| )(?<three>\d{1,4})\b", RegexOptions.IgnoreCase);
        public static string Parse(string str)
        {
            Match m = regex.Match(str);
            int matchCount = 0;
            while (m.Success)
            {
                Console.WriteLine("Match" + (++matchCount));
                for (int i = 1; i <= 4; i++)
                {
                    Group g = m.Groups[i];
                    Console.WriteLine("Group" + i + "='" + g + "'");
                    CaptureCollection cc = g.Captures;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        Capture c = cc[j];
                        System.Console.WriteLine("Capture" + j + "='" + c + "', Position=" + c.Index);
                    }
                }
                m = m.NextMatch();
            }
            return "";
        }
    }
}
