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
        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap job_DataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_DataMap = context.Trigger.JobDataMap;
            object obj = job_DataMap.Get("testJobDetail");

            JObject jObject = JObject.Parse(obj.ToString());

            var cookieString = Config.Cookie;
            var cookieString2 = Config.Cookie2;
            var url = jObject.GetValue("Url").ToString();
            var contentType = jObject.GetValue("ContentType").ToString();
            var shortCodeList = jObject.GetValue("ShortCodeList").ToArray();
            List<Task> taskLists = new List<Task>();
            for (int i = 0; i < shortCodeList.Length; i++)
            {

                int index = i;

                if (taskLists.Count(x => x.Status != TaskStatus.RanToCompletion) >= 20)
                {
                    Task.WaitAny(taskLists.ToArray());
                    taskLists = taskLists.Where(x => x.Status != TaskStatus.RanToCompletion).ToList();
                }
                else
                {
                    taskLists.Add(Task.Run(() =>
                    {
                        Console.WriteLine($"第 {index + 1} 轮任务开始...{DateTime.Now}");
                        var reqUrl = string.Format(url, shortCodeList[index]);

                        try
                        {
                            //获取post列表
                            //var result = TaskSchedulers._webUtils.DoGet(url: reqUrl, parameters: null, contentType: contentType, cookieStr: cookieString);
                            ////获取token
                            //if (string.IsNullOrWhiteSpace(TaskSchedulers.Token))
                            //{
                            //    TaskSchedulers.Token = CommonHelper.GetToken(ConfigPage.tokenUrl, ConfigPage.contentType, ConfigPage.jsonPars);
                            //}
                            //准备写入数据库
                        //    Dictionary<string, string> dicPars = new Dictionary<string, string>
                        //{
                        //    {"ShortCode",shortCodeList[index]?.ToString() },
                        //    {"OringinalJson",result }
                        //};
                        //    Dictionary<string, string> headers = new Dictionary<string, string>
                        // {
                        //     {"Authorization","Bearer "+TaskSchedulers.Token }
                        // };
                        //    var postResult = TaskSchedulers._webUtils.DoPost(ConfigPage.updateInstagramUserUrl, new Dictionary<string, string>(), contentType, JsonConvert.SerializeObject(dicPars), false, headers);
                        //    Console.WriteLine($"第 {index + 1} 轮任务返回结果...{postResult}");
                            //TaskSchedulers._postDataService.Create(new Data { Datas = result });
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            var message = $"报错: {ex.Message} 错误数据: 索引: {index} ，内容: {shortCodeList[index]}";
                            Console.WriteLine(message);
                            
                        }

                    }));
                }

                await Task.Delay(1000);

            }
        }
    }
}
