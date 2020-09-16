using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.TaskManager.Job;
using CrawlerConsole.token;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole.Job
{
    public class KolPostJob : CommandJob
    {
        public override async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(100);
            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
            IList<JData> listTasks = base.GetCommList("inspost");
            List<Task> taskLists = new List<Task>();
            await ExecuteAction(async () => {
                await Request(listTasks, webUtils);
            });
        }

        private async Task Request(IList<JData> listTasks, WebUtils webUtils)
        {
            await Task.Delay(100);
            List<Task> taskLists = new List<Task>();
            for (int i = 0; i < listTasks.Count; i++)
            {
                int index = i;
                string shortcode = JObject.Parse(listTasks[index].parameters?.ToString()).GetValue("ShortCode")?.ToString();
                if (taskLists.Count(x => x.Status != TaskStatus.RanToCompletion) >= 10)
                {
                    Task.WaitAny(taskLists.ToArray());
                    taskLists = taskLists.Where(x => x.Status != TaskStatus.RanToCompletion).ToList();
                }
                else
                {
                    taskLists.Add(Task.Run(() =>
                    {
                        Console.WriteLine($"{nameof(KolPostJob)}第 {index + 1} 轮任务开始...{DateTime.Now}");
                        var reqUrl = listTasks[index].targetUrl;
                        try
                        {
                            Thread.Sleep(3000);
                            //获取post列表
                            var result = webUtils.DoGet(url: reqUrl, parameters: null, contentType: "application/json", cookieStr: Config.Cookie);

                            //准备写入数据库
                            Dictionary<string, string> dicPars = new Dictionary<string, string>
                                {
                                    {"Shortcode",shortcode },
                                    {"OringinalJson",result }
                                };
                            Dictionary<string, string> headers = new Dictionary<string, string>
                                 {
                                     {"Authorization","Bearer "+TokenString }
                                 };
                            //Config.updateInstagramPostUrl
                            Thread.Sleep(3000);
                            var postResult = webUtils.DoPost("http://localhost:8088/Tarpa/InstagramPosts/UpdateInstagramPost", null, "application/json", JsonConvert.SerializeObject(dicPars), false, headers);
                            Console.WriteLine($"{nameof(KolPostJob)}->第 {index + 1} 轮任务返回结果...{postResult}");
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(KolPostJob)}报错: {ex.Message} 错误数据: 索引: {index} ，内容: {listTasks[index].id}";
                            CommonHelper.ConsoleAndLogger(message, CommonHelper.LoggerType.Error);
                        }

                    }));
                }
            }
        }
    }
}
