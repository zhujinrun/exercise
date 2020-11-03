using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.Selenium.Helper;
using Crawler.Service;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.TaskManager.Job;
using CrawlerConsole.token;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Quartz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class KolShortCodeJob : IJob
    {
  
        private static ConcurrentQueue<string> queueList = new ConcurrentQueue<string>(); //存放列表
        
        private static BaseJob baseJob = CustomApplicationService.GetService<BaseJob>();
        private static SeleniumHelper seleniumHelper = baseJob.seleniumHelper;
        private RemoteWebDriver driver;
        public KolShortCodeJob()
        {
            Console.WriteLine("constructor starting...");
            driver =  seleniumHelper.Login(Config.ParsingCookie(Config.Cookie),Config.igUrl,Config.iSLocalEnvironment);
        }
       
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("shortcodejob starting...");
            var listTasks = baseJob.GetCommList("instagged");

            try
            {
                CommonHelper.ConsoleAndLogger($"{nameof(KolShortCodeJob)}=>获取shortcode开始...{CommonHelper.GetSTime(Config.iSLocalEnvironment)}", CommonHelper.LoggerType.Info);
                bool Isinsert = true;
                var insertUrl = listTasks.FirstOrDefault().postBackUrl;

                Task.Run(() => {

                    CommonHelper.ConsoleAndLogger($"start insert to db",CommonHelper.LoggerType.Info);
                    InsertDB(Isinsert, insertUrl);
                });

                await Task.Run(() =>
                {
                    CommonHelper.ConsoleAndLogger($"start get post from ing", CommonHelper.LoggerType.Info);
                    foreach (var item in listTasks)
                    {
                        ShortCode(seleniumHelper, driver, item);
                    }
                    Isinsert = false;
                    CommonHelper.ConsoleAndLogger($"{nameof(KolShortCodeJob)}=>获取shortcode完成...{CommonHelper.GetSTime(Config.iSLocalEnvironment)}", CommonHelper.LoggerType.Info);
                });
            }
            catch
            {
                Quit(driver);
            }
        }

        private  void ShortCode(SeleniumHelper seleniumHelper,RemoteWebDriver driver, JData jData, string cls = "_bz0w")
        {
            driver.Url = driver.Url!= jData.targetUrl ? jData.targetUrl : driver.Url;
            long height = 0; //存放鼠标上一次执行后浏览器的高度
            bool isGoto = true;         //是否循环获取数据
            bool isFirst = true;          //第一次加载
            while (isGoto)
            {
                if (!isFirst)
                {
                    seleniumHelper.ScrollMouse(driver, 3000);
                }
                else
                {
                    isFirst = false;
                }
                object scrolHeight = seleniumHelper.GetScrollHeight(driver); //获取高度

                Thread.Sleep(3000);
                if (string.IsNullOrEmpty(scrolHeight?.ToString()))
                {
                    throw new Exception("获取数据长度为空");
                }
                long.TryParse(scrolHeight?.ToString(), out long tobeequal);
                object newScrolHeight = 0;   //存放鼠标滚动后高度
                long newTobeequal = 0;   //新的高度
                if (tobeequal == height)
                {
                    var nextDelay = TimeSpan.FromMilliseconds(10000);  // 重试3次
                    for (int i = 0; i != 3; ++i)
                    {
                        try
                        {
                            newScrolHeight = seleniumHelper.GetScrollHeight(driver);
                            long.TryParse(newScrolHeight?.ToString(), out newTobeequal);
                            if (newTobeequal != height)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(this.GetType() + "GetList", ex.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                            Quit(driver);
                            throw;
                        }
                        Thread.Sleep(nextDelay);
                        nextDelay = nextDelay + nextDelay;
                    }
                    if (newTobeequal == height)
                    {
                        isGoto = false;
                    }
                }
                height = tobeequal;
                EnqueueShortCode(cls,driver); //获取shortcode
            }
        }
        /// <summary>
        /// 获取队列
        /// </summary>
        /// <param name="cls"></param>
        private void EnqueueShortCode(string cls,RemoteWebDriver driver)
        {
            // xpath = //*[@id="react-root"]/section/main/div/div[3]/article/div[1]/div   
            IEnumerable<IWebElement> listres = driver.FindElementsByClassName(cls);          //获取最新数据

            foreach (var item in listres)
            {
                Thread.Sleep(2000);
                var href = string.Empty;
                try
                {
                    var div_a = item.FindElement(By.TagName("div a"));
                    if (div_a != null)
                    {
                        href = div_a.GetAttribute("href");

                    }
                    if (!string.IsNullOrEmpty(href))
                    {
                        var shortcode = href.Substring(href.LastIndexOf('/', (href.LastIndexOf("/") - 1)) + 1).TrimEnd('/');
                        if (!queueList.Contains(shortcode))
                        {
                            queueList.Enqueue(shortcode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = $"EnqueueShortCode{ ex.Message}";
                    LoggerHelper.Error(message);
                    ConsoleHelper.WriteLine(nameof(KolShortCodeJob), message, string.Empty, ConsoleColor.Red);
                    Quit(driver);
                }

            }
        }
        private void InsertDB(bool isGo,string insertUrl)
        {
            int index = 1;
            while (isGo)
            {
                if (queueList.Count > 0)
                {
                    queueList.TryDequeue(out string result);
                    //准备写入数据库       shortcode 截取 
                    Dictionary<string, string> dicPars = new Dictionary<string, string>
                                {
                                    {"ShortCode", result}
                                };
                    Dictionary<string, string> headers = new Dictionary<string, string>
                                 {
                                      {"Authorization",$"Bearer {baseJob.tokenString}" },
                                     {"content-type","application/json" }
                                 };
                    var postResult = baseJob.restClientHelper.PostRequestAsync(baseJob.restClient, "https://unibone.dev.heywind.cn", insertUrl, headers, dicPars).Result;
                    Console.WriteLine($"第 {index} 轮任务{result}写入OK...");
                    index++;
                }   
            }    
        }
        /// <summary>
        /// 退出
        /// </summary>
        private void Quit( RemoteWebDriver driver)
        {
            if (null != driver)
            {
                driver.Quit();
            }

        }
     
    }
}
