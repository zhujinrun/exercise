using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Crawler.API;
using Crawler.API.Model;
using Crawler.API.Selenuim;
using Crawler.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Org.BouncyCastle.Utilities;

namespace Crawler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly ILogger<ProviderController> _logger;
        private readonly PostDataService _postDataService;
        private readonly IOptionsMonitor<CookieInfoOptions> _optionsCookie;
        public ProviderController(ILogger<ProviderController> logger, IOptionsMonitor<CookieInfoOptions> cookieInfo, PostDataService postDataService)
        {
            _optionsCookie = cookieInfo;
            _logger = logger;
            _postDataService = postDataService;
        }
        [Route("InsertDataTest")]
        [HttpGet]
        public ContentResult InsertDataTest()
        {
            Data data = new Data();
            data.Datas = "test data";
            data.Id = ObjectId.GenerateNewId().ToString();
            Data result = _postDataService.Create(data);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }

        private static Queue<string> queueList = new Queue<string>(); //存放列表
        public static RemoteWebDriver driver;
        [Route("Login")]
        [HttpGet]
        public JsonResult Login(string url= "https://www.instagram.com/")
        {
            _logger.LogInformation("crawler start...");

            if (driver != null)
            {
                return new JsonResult(new
                {
                    Result = true,
                    Message = "启动驱动已经启动"
                });
            }
            driver = new SeleniumHelper().Login(_optionsCookie.CurrentValue, url);
            return new JsonResult(new
            {
                Result = null == driver ? false : true,
                Message = null == driver ? "启动驱动程序失败，请检查日志" : "启动驱动程序成功"
            });
        }


        [Route("GetList")]
        [HttpGet]
        public JsonResult GetList(string url= "https://www.instagram.com/tijneyewear/tagged/", string tableName = "default", string cls = "_bz0w")
        {
            if (driver.Url != url)
            {
                driver.Url = url;
            }
            long height = 0; //存放鼠标上一次执行后浏览器的高度
            bool isGoto = true;         //是否循环获取数据
            bool isFirst = true;          //第一次加载
            while (isGoto)
            {
                if (!isFirst)
                {
                    SeleniumHelper.ScrollMouse(driver, 3000);
                }
                else
                {

                    isFirst = false;
                }
                object scrolHeight = SeleniumHelper.GetScrollHeight(driver); //获取高度

                Thread.Sleep(100);
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
                            newScrolHeight = SeleniumHelper.GetScrollHeight(driver);
                            long.TryParse(newScrolHeight?.ToString(), out newTobeequal);
                            if (newTobeequal != height)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(this.GetType() + "GetList", ex.Message);
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
                EnqueueShortCode(cls); //获取shortcode

            }
            bool isInsert = true;
            InsertToDB(isInsert, queueList, tableName);    //所有数据获取完毕再写入数据库避免重复
            return new JsonResult(new
            {
                Result = true,
                Message = "OK"
            });
        }

        /// <summary>
        /// 获取网络请求列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Route("GetUrlList")]
        [HttpGet]
        public JsonResult GetUrlList(string url, string tableName)
        {
            long height = 0;
            bool isGoto = true;
            bool isFirst = true;
            while (isGoto)
            {

                if (!isFirst)
                {
                    SeleniumHelper.ScrollMouse(driver, 1000);
                }
                else
                {
                    isFirst = false;
                }
                object scrolHeight = SeleniumHelper.GetScrollHeight(driver); //滚动鼠标获取高度
                long.TryParse(scrolHeight?.ToString(), out long newHeight);

                object newScrolHeight = 0;   //存放鼠标滚动后高度
                long newTobeequal = 0;   //新的高度
                if (newHeight == height)
                {
                    var nextDelay = TimeSpan.FromMilliseconds(1000);
                    for (int i = 0; i != 3; ++i)
                    {
                        try
                        {
                            newScrolHeight = SeleniumHelper.GetScrollHeight(driver);
                            long.TryParse(newScrolHeight?.ToString(), out newTobeequal);

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(this.GetType() + "GetUrlList", ex.Message);
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
                else
                {
                    ReadOnlyCollection<object> netData = SeleniumHelper.ExecuteNetWorkList(driver);
                    QueueToList(netData);
                }
                height = newHeight;


            }
            InsertToDB(true, queueList, tableName);

            return new JsonResult(new
            {
                Result = true,
                Message = "OK"
            });
        }

        private void QueueToList(ReadOnlyCollection<object> netData)
        {
            foreach (Dictionary<String, object> item in netData)
            {
                foreach (KeyValuePair<string, object> sub in item)
                {

                    if (sub.Key.Equals("name"))
                    {

                        if (sub.Value.ToString().Contains("?query_hash"))
                        {
                            queueList.Enqueue(sub.Value.ToString());
                        }

                    }
                }
            }
        }

        [Route("Logout")]
        [HttpGet]
        public JsonResult Logout()
        {
            string message = "退出成功";
            bool result = true;
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "错误消息: " + ex.Message;
            }
            return new JsonResult(new
            {
                Result = result,
                Message = message
            });
        }

        private void EnqueueShortCode(string cls)
        {
            // xpath = //*[@id="react-root"]/section/main/div/div[3]/article/div[1]/div   
            IEnumerable<IWebElement> listres = driver.FindElementsByClassName(cls);          //获取最新数据  

            foreach (var item in listres)
            {
                var href = string.Empty;
                //var href = item.FindElement(By.TagName("div a"))?.GetAttribute("href");
                try
                {
                    var div_a = item.FindElement(By.TagName("div a"));
                    if (div_a != null)
                    {
                        href = div_a.GetAttribute("href");

                    }
                    if (!string.IsNullOrEmpty(href))
                    {
                        if (!queueList.Contains(href))
                        {
                            queueList.Enqueue(href);
                            Console.WriteLine($"{href}已读");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(this.GetType() + "EnqueueShortCode", ex.Message);
                    Console.WriteLine("错误提示: " + ex.Message);
                }

            }
        }
        int s_count = 0;
        private void InsertToDB(bool isInsert, Queue<string> queue, string tableName)
        {
            var str = string.Empty;
            while (isInsert)
            {
                if (queue.Count > 0)
                {
                    var ins = queue.Dequeue();
                    var res = ins.Substring(ins.LastIndexOf('/', (ins.LastIndexOf("/") - 1)) + 1).TrimEnd('/');
                    Data data = new Data
                    {
                         ShortCode = res
                    };
                    s_count += 1;
                    str += $"{res} \r\n";
                    //_postDataService.Create(data);
                }
                else
                {
                    isInsert = false;
                }
            }
            WriteToFile(str);
            Console.WriteLine($" {str}写入成功,已完成写入 {s_count} 条");
        }
        private static void WriteToFile(string data)
        {
            string path = "ShortCode.txt";
            using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.Default))
                {
                    streamWriter.WriteLine(data);
                }
            }
        }
    }
}
