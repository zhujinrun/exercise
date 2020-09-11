using Crawler.Logger;
using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.token;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class KolPostJob : IJob
    {
        private static string TokenString = string.Empty;
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(1);
            if (string.IsNullOrWhiteSpace(TokenString))
            {
                TokenHelper helper = ServiceDiExtension.GetService<TokenHelper>();
                TokenString = helper.GetToken(Config.uniboneTokenUrl, "application/json", Config.jsonPars);
            }
            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
          
            List<JData> listTasks = Program.jDatas.Where(x => x.action.ToLower().Equals("inspost")).OrderByDescending(x=>x.level).ToList();
            List<Task> taskLists = new List<Task>();
            string contentType = "application/json";
            
            for (int i = 0; i < listTasks.Count; i++)
            {
                int index = i;
                string shortcode = JObject.Parse(listTasks[index].parameters?.ToString()).GetValue("ShortCode")?.ToString();
                if (taskLists.Count(x => x.Status != TaskStatus.RanToCompletion) >= 20)
                {
                    Task.WaitAny(taskLists.ToArray());
                    taskLists = taskLists.Where(x => x.Status != TaskStatus.RanToCompletion).ToList();
                }
                else
                {
                    taskLists.Add( Task.Run(() =>
                    {
                        Console.WriteLine($"第 {index + 1} 轮任务开始...{DateTime.Now}");
                        var reqUrl = listTasks[index].targetUrl;

                        try
                        {
                            //获取post列表
                            var result = webUtils.DoGet(url: reqUrl, parameters: null, contentType: contentType, cookieStr: Config.Cookie);
                           
                            //准备写入数据库
                                    Dictionary<string, string> dicPars = new Dictionary<string, string>
                                {
                                    {"Shortcode",shortcode },
                                    {"OringinalJson",result }
                                };
                            Dictionary<string, string> headers = new Dictionary<string,string>
                                 {
                                     {"Authorization","Bearer "+TokenString }
                                 };
                            var postResult = webUtils.DoPost(Config.updateInstagramPostUrl, null, contentType, JsonConvert.SerializeObject(dicPars), false, headers);
                            Console.WriteLine($"{nameof(KolPostJob)}->第 {index + 1} 轮任务返回结果...{postResult}");
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            var message = $"报错: {ex.Message} 错误数据: 索引: {index} ，内容: {listTasks[index].id}";
                            LoggerHelper.Error(message);
                            Console.WriteLine(message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                    }));
                }
            }       
        }
    }
}
