using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Crawler.API.Filter;
using Crawler.Common.Selenuim;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Crawler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiRescourceFilterAttribute]
    public class InvokeController : ControllerBase
    {
        private readonly ILogger<InvokeController> _logger;
        public InvokeController(ILogger<InvokeController> logger)
        {
            _logger = logger;
        }
        public static RemoteWebDriver driver;
        //public InvokeController() { }

        [HttpGet]
        [Route("GetString")]
        public string Get()
        {
            return "Ok";
        }

        [HttpPost]
        [Route("LoginByUp")]
        public void LoginByUp(string url, string userName, string passWord, string inputUser, string inputPass)
        {
            url = "https://www.instagram.com/";
            inputUser = @"//*[@id='react-root']/section/main/article/div[2]/div[1]/div/form/div[2]/div/label/input";
            inputPass = @"//*[@id='react-root']/section/main/article/div[2]/div[1]/div/form/div[3]/div/label/input";
            var currPath = Directory.GetCurrentDirectory();
            SelenuimHelper helper = new SelenuimHelper();
            driver=helper.LoginByUp(url, userName, passWord, (url, name, pass) =>
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("start-maximized");
                options.PageLoadStrategy = PageLoadStrategy.Normal; //  等待加载返回load事件
                //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;   //返回 DOMContentLoaded 事件.
                RemoteWebDriver chromDriver = new ChromeDriver(currPath, options);
                chromDriver.Navigate().GoToUrl(url);
                #region 账号密码录入
                var userName = chromDriver.FindElementByXPath(inputUser);  //先判断页面是否加载完毕
                userName.SendKeys(name);

                var pwd = chromDriver.FindElementByXPath(inputPass);
                pwd.SendKeys(passWord);
                #endregion
                return chromDriver;
            }); 
        }

        [HttpPost]
        [Route("LoginBySession")]
        public void LoginBySession(string url = "https://www.instagram.com/", string domain = ".instagram.com")
        {
            SelenuimHelper helper = new SelenuimHelper();
            driver = helper.LoginBySession(url, domain);
        }
        [HttpGet]
        [Route("Click")]
        public void Click(string button)
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
        [HttpGet]
        [Route("GetInfo")]
        public string GetInfo(string xPath)
        {

            return "";
        }

        [HttpPost]
        [Route("GetAllRequest")]
        public void GetAllRequest(string url)
        {
            //获取js请求
            driver.Url = url;
            String scriptToExecute = "var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {}; var network = performance.getEntries() || {}; return network;";
            var netData = ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExecute);
            Console.WriteLine(netData);
        }

        [HttpPost]
        [Route("ExecuteJs")]
        public string ExecuteJs(string js)
        {
            IJavaScriptExecutor jsRequest = (IJavaScriptExecutor)driver;
            return (string)jsRequest.ExecuteScript(js);
        }

    }
}
