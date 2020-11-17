using Crawler.Common;
using Crawler.Models;
using Crawler.Service;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.TaskManager.Job;
using CrawlerConsole.token;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class KolProfileJob : IJob
    {
        private BaseJob baseJob;
        public KolProfileJob()
        {
            baseJob = CustomApplicationService.GetService<BaseJob>();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            IList<JData> listTasks = baseJob.GetCommList("insprofile");
            for (int i = 0; i < listTasks.Count; i++)
            {
                int index = i;
                Console.WriteLine($"{nameof(KolProfileJob)}第 {index + 1} 轮任务开始...{CommonHelper.GetSTime(Config.iSLocalEnvironment)}");
                var reqUrl = listTasks[index].targetUrl;
                var result = string.Empty;
                try
                {
                   result = await baseJob.restClientHelper.GetRequestAsync(client:baseJob.restClient, host: Config.igUrl, url:reqUrl.Replace(Config.igUrl,""),new Dictionary<string, string>() { { "Cookie", Config.Cookie } });
                    Console.WriteLine("请求profile完成");
                }
                catch (Exception ex)
                {
                    var message = $"{nameof(KolPostJob)}获取shortcode报错: {ex.Message} 错误数据: 内容: {listTasks[i].id}";
                    CommonHelper.ConsoleAndLogger(message, CommonHelper.LoggerType.Error);
                }
                //获取post列表

                if (!string.IsNullOrEmpty(result) && result.Length > 2 &&!result.Equals("error"))
                {
                    //准备写入数据库
                    Dictionary<string, string> dicPars = new Dictionary<string, string>
                                    {
                                    {"OringinalJson",result }
                                    };
                    Dictionary<string, string> headers = new Dictionary<string, string>
                                 {
                                    {"Authorization",$"Bearer {baseJob.tokenString}" },
                                     {"content-type","application/json" }
                                 };
                    try
                    {
                        //报错shortcode是null ,检查队列url  http://localhost:8088  Config.unUrl
                        var postResult = await baseJob.restClientHelper.GetRequestAsync(client:baseJob.restClient, Config.unUrl , listTasks[index].postBackUrl, headers,dicPars);
                        Console.WriteLine($"{nameof(KolProfileJob)}第 {index + 1} 轮任务返回结果...{postResult}");
                    }
                    catch (Exception ex)
                    {
                        var message = $"{nameof(KolProfileJob)}写入数据库报错: {ex.Message} 错误数据";
                        CommonHelper.ConsoleAndLogger(message, CommonHelper.LoggerType.Error);
                    }
                }
            }
        }
    }
}
