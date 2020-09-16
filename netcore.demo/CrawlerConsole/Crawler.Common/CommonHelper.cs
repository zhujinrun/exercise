using Crawler.Logger;
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

        public static void ConsoleAndLogger(string message,LoggerType loggerType)
        {
            var color =  Console.ForegroundColor;
            var current = ConsoleColor.White;
            switch (loggerType)
            {
                case LoggerType.Error:
                    LoggerHelper.Error(message);
                    current = ConsoleColor.Red;
                    break;            
                case LoggerType.Warn:
                    LoggerHelper.Warn(message);
                    break;
                case LoggerType.Info:
                default:
                    LoggerHelper.Info(message);
                    break;
            }
            ConsoleHelper.WriteLine(LoggerType.Error.ToString(), message, "", current);
            Console.ForegroundColor = color;
        }
        public enum LoggerType
        {
            Error,
            Info,
            Warn
        }
    }
}
