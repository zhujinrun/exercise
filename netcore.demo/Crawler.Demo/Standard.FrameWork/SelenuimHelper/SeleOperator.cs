using Crawler.Common;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Standard.FrameWork.SelenuimHelper
{
    public class SeleOperator
    {
        public RemoteWebDriver Login(string jsonCookie,string url= "https://www.instagram.com/")
        {
            if (!ComMethod.IsUrl(url))
            {
                throw new Exception($"this {url} url address is error");
            }
            RemoteWebDriver driver=null;
            try
            {
                driver = new ChromeDriver(Directory.GetCurrentDirectory());
                driver.Url = url;

                JObject jObject = JObject.Parse(jsonCookie);
                foreach (var item in jObject)
                {
                    driver.Manage().Cookies.AddCookie(new Cookie(item.Key, item.Value.ToString()));
                }
                driver.Url = url;
                return driver;
            }
            catch(Exception ex)
            {
                if (null != driver)
                {
                    driver.Quit();
                }
                throw ex;
            }
            #region json格式cookie
            /*
               {
                "ig_did":"2822361A-5BAD-4025-A265-6A4E0C502C9E",
                "mid":"XxJ6DwALAAEdhBWbFY1uu6R2hLDx",
                "rur":"PRN",
                "csrftoken":"EieOui4LSSFo8ZI8PPvrUx9fof05qgcR",
                "ds_user_id":"39391854053",           
                "sessionid":"39391854053%3AcJaBsvZxXXvy6W%3A2"
                }
                 */
            #endregion
        }
    }
}
