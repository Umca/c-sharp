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

        static Regex reg = new Regex(@"\b(?<day>\d{1,4})[-\/. ](?<month>(\d{1,2}|\w+))[-\/. ](?<year>\d{1,4})\b", RegexOptions.IgnoreCase);
        static ArrayList groupnames = new ArrayList() { "day", "month", "year" };
        static List<String> _monthes = new List<string>{ "January" , "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
        public static string Parse(string str)
        {
            int startIdx = 0;
            int len = 0;
            StringBuilder strbuild = new StringBuilder();
            MatchCollection matches = reg.Matches(str);
            if(matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    len = m.Index - startIdx;
                    string temp = str.Substring(startIdx, len);
                    strbuild.Append(temp);
                    string date = FormatDate(m);
                    strbuild.Append(date);
                    startIdx = m.Index + m.Length;
                }

                return strbuild.Append(str.Substring(startIdx)).ToString();
            }
            return str;
            
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
                            for(int i = 0; i<_monthes.Count; i++)
                            {
                                if (_monthes[i].Contains(group.Value))
                                {
                                    d.month = (i+1).ToString().PadLeft(2, '0');
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
