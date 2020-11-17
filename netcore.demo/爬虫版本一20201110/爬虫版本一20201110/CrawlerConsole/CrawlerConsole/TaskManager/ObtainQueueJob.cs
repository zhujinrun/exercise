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
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class ObtainQueueJob : IJob
    {
        private BaseJob baseJob;
       
        public ObtainQueueJob() {
            baseJob =  CustomApplicationService.GetService<BaseJob>();
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                IDictionary<string, string> headers = new Dictionary<string, string>
                {
                   {"Authorization","Bearer "+baseJob.tokenString }
                };
                //获取队列列表
                CommonHelper.ConsoleAndLogger($"{nameof(ObtainQueueJob)}=>获取队列列表开始... {CommonHelper.GetSTime(Config.iSLocalEnvironment)}", CommonHelper.LoggerType.Info);
                string commandQueueString2 = baseJob.webUtils.DoPost(Config.commandQueueListUrl, null, "application/json", "{}", false, headers);

                //处理列表
                StorageQueue(commandQueueString2, 2);
                //存放队列
                CommonHelper.ConsoleAndLogger($"{nameof(ObtainQueueJob)}=>获取列表完成... {CommonHelper.GetSTime(Config.iSLocalEnvironment)}", CommonHelper.LoggerType.Info);
                
            }
            catch(Exception ex)
            {
                CommonHelper.ConsoleAndLogger(ex.Message, CommonHelper.LoggerType.Error);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取列表或者单条数据
        /// </summary>
        /// <param name="commandQueueString"></param>
        private static void StorageQueue(string commandQueueString, int mark)
        {
            List<JData> list = new List<JData>();
            ResponseMessage responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(commandQueueString);
            //列表 单条做处理
            JData jData = null;
            List<JData> jDatas = null;
            if (mark == 1)
            {
                jData = JsonConvert.DeserializeObject<JData>(JsonConvert.SerializeObject(responseMessage.data));
            }
            else
            {
                jDatas = JsonConvert.DeserializeObject<List<JData>>(JsonConvert.SerializeObject(responseMessage.data));
            }

            if (jData != null)
            {
                Program.jDatas.Add(jData);
            }
            if (jDatas != null && jDatas.Count > 0)
            {
                jDatas.ForEach(x => Program.jDatas.Add(x));
            }
        }
    }
}
