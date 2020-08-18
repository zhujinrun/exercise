using Crawler.Common.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Crawler.Common.Selenuim
{
    public class SelenuimHelper
    {

        /// <summary>
        /// 登录获取driver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public RemoteWebDriver LoginByUp(string url,string user,string password,Func<string,string,string,RemoteWebDriver> func)
        {
            return func.Invoke(url, user, password);
        }
        
        /// <summary>
        /// 通过添加session登录
        /// </summary>
        /// <returns></returns>
        public RemoteWebDriver LoginBySession(string url ,string domain)
        {
            RemoteWebDriver driver = null;
            try
            {
                var cookiePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
                var encKeyPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Local State";


                driver = new ChromeDriver(@"C:\Users\lzx\source\repos\Crawler.Demo\Crawler.API");

                driver.Navigate().GoToUrl(url);

                ReadCookie rc = new ReadCookie();
                SeleCookie sc = rc.GetCookies(cookiePath, encKeyPath);
                var collection = sc.GetType().GetProperties().Where(x => !x.Name.Equals("Item") && !x.Name.Equals("urlgen")).Select(x => x.Name).GetEnumerator();
                while (collection.MoveNext())
                {
                    var current = collection.Current;
                    driver.Manage().Cookies.AddCookie(new Cookie(current, sc[current]));
                }
                driver.Url = url;
            }
            catch (Exception ex)
            {
                driver.Quit();
                throw ex;
            }
            finally
            {
                //driver.Quit();
            }
            return driver;
        }
        /// <summary>
        /// 通过xpath获取元素信息
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string FindElementByXPath(RemoteWebDriver driver,string xpath)
        {
            return driver.FindElementByXPath(xpath).Text;
        }

        /// <summary>
        /// 通过css样式获取元素
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string FindElementByCSs(RemoteWebDriver driver, string xpath)
        {
            return driver.FindElementByXPath(xpath).Text;
        }

        /// <summary>
        /// 通过xpath获取元素列表
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindElementsByXPath(RemoteWebDriver driver, string xpath)
        {
            return driver.FindElementsByXPath(xpath);
        }

        /// <summary>
        /// 通过css样式获取列表
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindElementsByCSs(RemoteWebDriver driver, string xpath)
        {
            return driver.FindElementsByXPath(xpath);
        }

    }
}
