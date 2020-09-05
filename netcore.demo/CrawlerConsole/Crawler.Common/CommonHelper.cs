using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Crawler.Common
{
    public class CommonHelper
    {
       
        public static bool IsUrl(string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
