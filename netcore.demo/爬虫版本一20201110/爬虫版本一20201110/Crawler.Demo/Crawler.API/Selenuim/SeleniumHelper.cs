using Crawler.API.Cookies;
using Crawler.API.Helper;
using Crawler.API.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.API.Selenuim
{
    public class SeleniumHelper
    {
        public RemoteWebDriver Login(CookieInfoOptions jsonCookie, string url)
        {
          
            if (!CommonHelper.IsUrl(url))
            {
                throw new Exception($"this {url} url address is error");
            }
            RemoteWebDriver driver = null;
            try
            {
                var chromeOptions = new ChromeOptions();

                chromeOptions.AddArguments("start-maximized");
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);  //禁止加载图片 可用
                chromeOptions.AddArguments("--no-sandbox");
                chromeOptions.AddArgument("disable-gpu");
                chromeOptions.AddArgument("--headless"); //后台运行模式
                Task.Delay(5000).ContinueWith((o) =>
                {
                    driver = new ChromeDriver(chromeOptions);
                }).GetAwaiter().GetResult();

                if (null == driver)
                {
                    return null;
                }
                driver.Url = url;
                Thread.Sleep(5000);

                foreach (var item in jsonCookie.GetType().GetProperties())
                {
                    var value = item.GetValue(jsonCookie);
                    if (value != null)
                        driver.Manage().Cookies.AddCookie(new Cookie(item.Name, value.ToString()));
                }
                driver.Url = url;
                Thread.Sleep(5000);
                return driver;
            }
            catch (Exception ex)
            {
                if (null != driver)
                {
                    driver.Quit();
                }
                throw ex;
            }
        }
        /// <summary>
        /// 登录获取driver
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public RemoteWebDriver LoginByUp(string url, string user, string password, Func<string, string, string, RemoteWebDriver> func)
        {
            return func.Invoke(url, user, password);
        }

        /// <summary>
        /// 通过添加session登录
        /// </summary>
        /// <returns></returns>
        public RemoteWebDriver LoginBySession(string url, string domain)
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
        public string FindElementByXPath(RemoteWebDriver driver, string xpath)
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

        /// <summary>
        /// 获取浏览器高度
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static object ExecuteRtnScrollHeight(RemoteWebDriver driver)
        {
            return driver.ExecuteScript("return  document.body.scrollHeight;");
        }

        /// <summary>
        /// 获取网络请求列表
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<object> ExecuteNetWorkList(RemoteWebDriver driver)
        {
            String scriptToExecute = "var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}; var network = performance.getEntries() || {}; return network;";
            ReadOnlyCollection<object> netData = ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute) as ReadOnlyCollection<object>;
            return netData;
        }

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        public static void ScrollMouse(RemoteWebDriver driver, int millisecondsTimeout)
        {
            object obj = driver.ExecuteScript("scroll(0,100000000)");   //鼠标滚动
            //driver.ExecuteScript("document.body.scrollTop = document.body.scrollHeight;");
            Thread.Sleep(millisecondsTimeout);
        }

        /// <summary>
        /// 获取页面高度
        /// </summary>
        /// <returns></returns>
        public static object GetScrollHeight(RemoteWebDriver driver)
        {
            var scrolHeight = driver.ExecuteScript("return  document.body.scrollHeight;");    //获取高度
            return scrolHeight;
        }

        public void Click(string button, RemoteWebDriver driver)
        {
            if (string.IsNullOrWhiteSpace(button))
                button = @"//*[@id='react-root']/section/main/article/div[2]/div[1]/div/form/div[4]/button/div";
            var btnLogin = driver.FindElementByXPath(button);
            Thread.Sleep(1000);
            if (btnLogin != null && btnLogin.Displayed == true)
            {
                btnLogin.Click();
            }
        }
    }
}