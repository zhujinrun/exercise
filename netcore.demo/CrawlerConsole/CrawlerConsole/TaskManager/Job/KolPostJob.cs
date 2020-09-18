using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.TaskManager.Job;
using CrawlerConsole.token;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole.Job
{

    public class KolPostJob : CommandJob
    {
        private static readonly object objLock = new object();
        public async override Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(100);
            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
            IList<JData> listTasks = base.GetCommList("inspost");
            ExecuteAction(() =>
           {
               Request(listTasks, webUtils);
           });
        }
        private void Request(IList<JData> listTasks, WebUtils webUtils)
        {
            List<Task> taskLists = new List<Task>();
            int index = 0;
            foreach (var jData in listTasks)
            {
                
                if (taskLists.Where(x => x.Status != TaskStatus.RanToCompletion).Count() >= 10)
                {
                    Task.WaitAny(taskLists.ToArray());
                    taskLists = taskLists.Where(x => x.Status != TaskStatus.RanToCompletion).ToList();
                }
                else
                {
                    taskLists.Add(Task.Run(() =>
                    {

                        JData data = jData;
                        string shortcode = JObject.Parse(data.parameters?.ToString()).GetValue("ShortCode")?.ToString();

                        Console.WriteLine($"{nameof(KolPostJob)} {data.id} 任务开始...{DateTime.Now}");
                        var reqUrl = data.targetUrl;
                        var result = string.Empty;
                        Thread.Sleep(8000);
                        //获取post列表
                        try
                        {
                            lock (objLock)
                            {
                                result = webUtils.DoGet(url: reqUrl, parameters: null, contentType: "application/json", cookieStr: Config.Cookie);
                            }     
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(KolPostJob)}获取shortcode报错: {ex.Message} 错误数据: 内容: {data.id}";
                            CommonHelper.ConsoleAndLogger(message, CommonHelper.LoggerType.Error);
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
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

                            Thread.Sleep(3000);
                            try
                            {
                                var postResult = string.Empty;
                                lock (objLock)
                                {
                                    postResult = webUtils.DoPost(Config.updateInstagramPostUrl, null, "application/json", JsonConvert.SerializeObject(dicPars), false, headers);
                                }
                                Console.WriteLine($"{nameof(KolPostJob)}-> {data.id} 任务返回结果...{postResult}");
                            }
                            catch (Exception ex)
                            {
                                var message = $"{nameof(KolPostJob)}写入数据库报错: {ex.Message} 错误数据: 内容: {data.id}";
                                CommonHelper.ConsoleAndLogger(message, CommonHelper.LoggerType.Error);
                            }
                        }
                    }));
                }
                Console.WriteLine($"任务{index}完成");
                index++;
            }
        }    
    }
}
