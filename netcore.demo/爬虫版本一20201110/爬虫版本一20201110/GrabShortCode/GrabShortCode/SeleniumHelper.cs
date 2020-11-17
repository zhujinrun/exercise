using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrabShortCode
{
    public class SeleniumHelper
    {
       
            private RemoteWebDriver driver = null;
            public RemoteWebDriver Login(CookieInfoOptions jsonCookie, string url = "https://www.instagram.com/", bool iSLocalEnvironment = false)
            {
                Console.WriteLine("Loginng starting...");
                if (driver != null)
                {
                    return driver;
                }
                try
                {
                    var chromeOptions = new ChromeOptions();

                    chromeOptions.AddArguments("start-maximized");
                    chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);  //禁止加载图片 可用
                    chromeOptions.AddArguments("--no-sandbox");
                   
                    chromeOptions.AddArgument("disable-gpu");
                    chromeOptions.AddArgument("--headless"); //后台运行模式

                    Console.WriteLine("Creating starting...");
                    driver = new ChromeDriver(Directory.GetCurrentDirectory(), chromeOptions);

                    Console.WriteLine("Crateing Completed...");
                    if (null == driver)
                    {
                        throw new Exception("驱动启动错误...检查重试!");
                    }
                    driver.Url = url;
                    Thread.Sleep(3000);

                    foreach (var item in jsonCookie.GetType().GetProperties())
                    {
                        var value = item.GetValue(jsonCookie);
                        if (value != null)
                            driver.Manage().Cookies.AddCookie(new Cookie(item.Name, value.ToString()));
                    }
                    driver.Url = url;
                    Thread.Sleep(2000);
                    #region 后台模式禁用 
                   
                    #endregion
                    Console.WriteLine("return driver...");
                    return driver;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"错误 :  {ex.Message}");
                    if (null != driver)
                    {
                        driver.Quit();
                        driver.Dispose();
                        driver = null;
                    }
                    throw ex;
                }
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
            public object ExecuteRtnScrollHeight(RemoteWebDriver driver)
            {
                return driver.ExecuteScript("return  document.body.scrollHeight;");
            }

            /// <summary>
            /// 获取网络请求列表
            /// </summary>
            /// <param name="driver"></param>
            /// <returns></returns>
            public ReadOnlyCollection<object> ExecuteNetWorkList(RemoteWebDriver driver)
            {
                String scriptToExecute = "var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}; var network = performance.getEntries() || {}; return network;";
                ReadOnlyCollection<object> netData = ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute) as ReadOnlyCollection<object>;
                return netData;
            }



            /// <summary>
            /// 鼠标滚动
            /// </summary>
            public void ScrollMouse(RemoteWebDriver driver, int millisecondsTimeout)
            {
                object obj = driver.ExecuteScript("scroll(0,100000000)");   //鼠标滚动
                                                                            //driver.ExecuteScript("document.body.scrollTop = document.body.scrollHeight;");
                Thread.Sleep(millisecondsTimeout);
            }

            /// <summary>
            /// 获取页面高度
            /// </summary>
            /// <returns></returns>
            public object GetScrollHeight(RemoteWebDriver driver)
            {
                var scrolHeight = driver.ExecuteScript("return  document.body.scrollHeight;");    //获取高度
                return scrolHeight;
            }

            public void Click(string button, RemoteWebDriver driver)
            {
                if (string.IsNullOrWhiteSpace(button))
                    button = @"/html/body/div[4]/div/div/div/div[3]/button[2]";
                var btnLogin = driver.FindElementByXPath(button);
                Thread.Sleep(1000);
                if (btnLogin != null && btnLogin.Displayed == true)
                {
                    btnLogin.Click();
                }
            }
        }
    
}
