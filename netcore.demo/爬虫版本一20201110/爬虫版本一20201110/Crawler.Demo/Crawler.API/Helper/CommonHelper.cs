using Crawler.API.Model;
using Crawler.Utility.HttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.API.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="jsonPars"></param>
        /// <returns></returns>
        public static string GetToken(string url, string contentType, string jsonPars)
        {
            WebUtils webUtils = new WebUtils();
            var result = webUtils.DoPost(url, new Dictionary<string, string>(), contentType, jsonPars, false, null);
            var token = JObject.Parse(result).GetValue("data")["token"]?.ToString();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("获取token有误");
            }
            return token;
        }

        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 解析cookie返回CookieInfoOptions对象
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static CookieInfoOptions ParsingCookie(string strs)
        {
            var json = string.Empty;
            var collection = strs.Split(";");
            foreach (var str in collection)
            {
                var arr = str.Split("=");
                arr[0] = arr[0].Trim();
                if (!string.IsNullOrWhiteSpace(arr[1]))
                {
                    if (arr[0] == "urlgen")
                    {

                        json += $"\"{arr[0]}\":{arr[1]}";
                    }
                    else
                    {
                        json += $"\"{arr[0]}\":\"{arr[1]}\",";
                    }
                }
            }
            return JsonConvert.DeserializeObject<CookieInfoOptions>("{" + json.TrimEnd(',') + "}");
        }
    }
}