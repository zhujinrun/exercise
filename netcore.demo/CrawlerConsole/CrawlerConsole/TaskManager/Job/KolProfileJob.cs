using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{

    public class KolProfileJob : CommandJob
    {
        public async override Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(100);
            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
            IList<JData> listTasks = base.GetCommList("insprofile");
            List<Task> taskLists = new List<Task>();
            await ExecuteAction(async () => {
                await Request(listTasks, webUtils);
            });
        }

        public async Task Request(IList<JData> listTasks, WebUtils webUtils)
        {
            await Task.Delay(100);
            List<Task> taskLists = new List<Task>();
            for (int i = 0; i < listTasks.Count; i++)
            {
                int index = i;
                Console.WriteLine($"第 {index + 1} 轮任务开始...{DateTime.Now}");
                var reqUrl = listTasks[index].targetUrl;

                try
                {
                    //获取post列表
                    var result = webUtils.DoGet(url: reqUrl, parameters: null, contentType: "application/json", cookieStr: Config.Cookie);
    
                //准备写入数据库
                    Dictionary<string, string> dicPars = new Dictionary<string, string>
                                    {
                                    //{"Shortcode",shortcode },
                                    {"OringinalJson",result }
                                    };
                    Dictionary<string, string> headers = new Dictionary<string, string>
                                 {
                                     {"Authorization","Bearer "+TokenString }
                                 };
                    //报错shortcode是null ,检查队列url  http://localhost:8088  Config.unUrl
                    var postResult = webUtils.DoPost(Config.unUrl + listTasks[index].postBackUrl, null, "application/json", JsonConvert.SerializeObject(dicPars), false, headers);
                    Console.WriteLine($"第 {index + 1} 轮任务返回结果...{postResult}");
                    index++;
                }
                catch (Exception ex)
                {
                    var message = $"报错: {ex.Message} 错误数据: 索引: {index}";
                    LoggerHelper.Error(message);
                    ConsoleHelper.WriteLine(nameof(KolProfileJob), message, string.Empty, ConsoleColor.Red);

                }
            }
           
        }
    }  
}
