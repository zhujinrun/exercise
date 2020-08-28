using Crawler.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.API.Filter
{
    public class ApiRescourceFilterAttribute : Attribute, IResourceFilter, IFilterMetadata
    {
        private static Dictionary<string, IActionResult> CustomCache = new Dictionary<string, IActionResult>();
        //public static RemoteWebDriver driver;
        /// <summary>
        /// action执行后处理
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            string path = context.HttpContext.Request.Path;
            Console.WriteLine($"请求地址为:{path} , 参数： {System.Web.HttpUtility.UrlDecode(context.HttpContext.Request.QueryString.ToString())}");

            string key = context.HttpContext.Request.Path;
            if (!CustomCache.ContainsKey(key))
            {
                CustomCache.Add(key, context.Result);
            }
            ProviderController.driver.Quit();
        }

        /// <summary>
        /// action执行前处理
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string key = context.HttpContext.Request.Path;
            if (CustomCache.ContainsKey(key))
            {
                context.Result = CustomCache[key];
                return;
            }         

        }
    }
}
