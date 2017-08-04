using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Tool
{
    public static class StringExtensionTool
    {
       
        public static bool IsNumber(this string strobj)
        {
            if (strobj == null)
                return false;

            if (Regex.IsMatch(strobj, @"^\d+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetWeek(this int weekFlag)
        {
            switch (weekFlag)
            {
                case 0 :
                    return "星期日";
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                default:
                    return string.Empty;
            }
        }
    }
}
