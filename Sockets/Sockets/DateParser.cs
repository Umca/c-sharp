using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Sockets
{

    static class DateParser
    {
        struct MyDate
        {
            public string delim;
            public string day;
            public string month;
            public string year;

            public override string ToString()
            {
                return day + delim + month + delim + year;
            }
        }

        static Regex reg = new Regex(@"\b(?<day>\d{1,4})(-|\/|.| )(?<month>(\d{1,2}|\w+))(-|\/|.| )(?<year>\d{1,4})\b", RegexOptions.IgnoreCase);
        static ArrayList groupnames = new ArrayList() { "day", "month", "year" };
        static Dictionary<String, ArrayList> monthes = new Dictionary<String, ArrayList>()
        {
            {"01", new ArrayList(){"January", "Jan" } },
            {"02", new ArrayList(){"February", "Feb" } },
            {"03", new ArrayList(){"March", "Mar" } },
            {"04", new ArrayList(){"April", "Apr" } },
            {"05", new ArrayList(){"May", "May" } },
            {"06", new ArrayList(){"June", "Jun" } },
            {"07", new ArrayList(){"July", "Jul" } },
            {"08", new ArrayList(){"August", "Aug" } },
            {"09", new ArrayList(){"September", "Sep" } },
            {"10", new ArrayList(){"October", "Oct" } },
            {"11", new ArrayList(){"November", "Nov" } },
            {"12", new ArrayList(){"December", "Dec" } },
        };
        public static string Parse(string str)
        {
            int startIdx = 0;
            int len = 0;
            StringBuilder strbuild = new StringBuilder();
            MatchCollection matches = reg.Matches(str);
            foreach (Match m in matches)
            {
                Console.WriteLine(m.Value);
                Console.WriteLine(m.Index);
                len = m.Index - startIdx;
                string temp = str.Substring(startIdx, len);
                strbuild.Append(temp);
                string date = FormatDate(m);
                strbuild.Append(date);
                startIdx = m.Index + m.Length;
            }
            return strbuild.ToString();
        }
        static string FormatDate(Match m)
        {
            var d = new MyDate();
            d.delim = "/";

            foreach (Group group in m.Groups)
            {
                if (groupnames.Contains(group.Name))
                {
                    if(group.Name.CompareTo("month") == 0)
                    {
                        int tempMonth;
                        if(int.TryParse(group.Value, out tempMonth))
                        {
                            d.month = group.Value.PadLeft(2, '0');
                        }
                        else
                        {
                            foreach(var entry in monthes)
                            {
                                if (entry.Value.Contains(group.Value))
                                {
                                    d.month = entry.Key;
                                }
                            }
                        }
                    }
                    else if(group.Value.Length == 4)
                    {
                        d.year = group.Value;
                    }
                    else
                    {
                        d.day = group.Value.PadLeft(2, '0');
                    }
                }
            }
            return d.ToString();
        }
    }
}
