using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crawler.API;
using Crawler.API.Helper;
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
using OpenQA.Selenium.Chrome;
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
        private IConfiguration configuration;
        public ProviderController(ILogger<ProviderController> logger, IOptionsMonitor<CookieInfoOptions> cookieInfo, PostDataService postDataService,IConfiguration configuration)
        {
            this.configuration = configuration;
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
            driver = new SeleniumHelper().Login(CommonHelper.ParsingCookie(configuration["Cookie"]), url);
            return new JsonResult(new
            {
                Result = null == driver ? false : true,
                Message = null == driver ? "启动驱动程序失败，请检查日志" : "启动驱动程序成功"
            });
        }


        [Route("GetList")]
        [HttpGet]
        public JsonResult GetList(string url, string tableName = "default", string cls = "_bz0w")
        {
            
            url = "https://www.instagram.com/tijneyewear/tagged/";
            //PageScreenshotScroll(url, @"C: \Users\lzx\source\repos\Crawler.Demo\Crawler.API\bin\Debug\netcoreapp3.1\test.txt"); //测试
            if (driver.Url != url)
            {
                driver.Url = url;
            }
            long height = 0; //存放鼠标上一次执行后浏览器的高度
            bool isGoto = true;         //是否循环获取数据
            bool isFirst = true;          //第一次加载
            int index = 1;
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
                Console.WriteLine($"页面更新后高度为:{scrolHeight}, 当前第{index}页");
                Thread.Sleep(1000);
                if (string.IsNullOrEmpty(scrolHeight?.ToString()))
                {
                    throw new Exception("获取数据长度为空");
                }
                long.TryParse(scrolHeight?.ToString(), out long tobeequal);
                object newScrolHeight = 0;   //存放鼠标滚动后高度
                long newTobeequal = 0;   //新的高度
                if (tobeequal == height)
                {
                    var nextDelay = TimeSpan.FromMilliseconds(3000);  // 重试3次
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
                index++;
            }
            //bool isInsert = true;
            //InsertToDB(isInsert, queueList, tableName);    //所有数据获取完毕再写入数据库避免重复
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
                            var res = href.Substring(href.LastIndexOf('/', (href.LastIndexOf("/") - 1)) + 1).TrimEnd('/');
                            WriteAllData(res + " ");
                            Console.WriteLine($"已写入{res}");
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
                    //_postDataService.Create(data);  
                    str += $"{res}/r/n";

                }
                else
                {
                    isInsert = false;
                }
            }
            WriteAllData(str);
            Console.WriteLine("写入文件完毕!");
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private void WriteAllData(string data)
        {
            using (Stream stream = new FileStream(Directory.GetCurrentDirectory()+"/test.txt", FileMode.OpenOrCreate))
            {
                byte[] block = Encoding.UTF8.GetBytes(data);
                stream.Write(block, 0, block.Length);
            }
        }
    }
}