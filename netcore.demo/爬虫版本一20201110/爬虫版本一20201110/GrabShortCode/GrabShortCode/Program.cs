using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrabShortCode
{
    class Program
    {
        private static int s_count = 0;
        static void Main(string[] args)
        {
            SeleniumHelper seleniumHelper = new SeleniumHelper();
            var driver = seleniumHelper.Login(new CookieInfoOptions(), "https://www.instagram.com/");
            ShortCode(seleniumHelper, driver, "_bz0w");
            Console.Read();

        }

        private static void ShortCode(SeleniumHelper seleniumHelper, RemoteWebDriver driver, string cls = "_bz0w")
        {
            driver.Url = "https://www.instagram.com/tijneyewear/tagged/";
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

                Thread.Sleep(1000);
                if (string.IsNullOrEmpty(scrolHeight?.ToString()))
                {
                    throw new Exception("获取数据长度为空");
                }
                long.TryParse(scrolHeight?.ToString(), out long tobeequal);
                object newScrolHeight = 0;   //存放鼠标滚动后高度
                long newTobeequal = 0;   //新的高度
                #region 失败重试3次
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
                            Console.WriteLine(ex.Message);
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
                #endregion
                height = tobeequal;
                EnqueueShortCode(cls, driver); //获取shortcode
            }
        }

        private static void EnqueueShortCode(string cls, RemoteWebDriver driver)
        {
            // xpath = //*[@id="react-root"]/section/main/div/div[3]/article/div[1]/div   
            IEnumerable<IWebElement> listres = driver.FindElementsByClassName(cls);          //获取最新数据
            var str = string.Empty; 
            foreach (var item in listres)
            {
                Thread.Sleep(500);
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
                            s_count += 1;
                            str +=$"{shortcode} \r\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = $"EnqueueShortCode{ ex.Message}";
                    Console.WriteLine($"error{message}");
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
       
        private static ConcurrentQueue<string> queueList = new ConcurrentQueue<string>(); //存放列表
    }
}
