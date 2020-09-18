using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.token;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{
    public class ObtainQueueJob : CommandJob
    {
        public async override Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(100);
             ExecuteAction();
        }

        private  void ExecuteAction()
        {
            Task.Delay(100);
            try
            {
                WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
                IDictionary<string, string> headers = new Dictionary<string, string>
                {
                   {"Authorization","Bearer "+TokenString }
                };
                IDictionary<string, object> parmeters = new Dictionary<string, object>
                {
                   {"Level",10},
                   { "Action",null}
                };

                //获取队列列表
                CommonHelper.ConsoleAndLogger($"{nameof(ObtainQueueJob)}=>获取队列列表开始... {DateTime.Now}", CommonHelper.LoggerType.Info);
                string commandQueueString2 = webUtils.DoPost(Config.commandQueueListUrl, null, "application/json", "{}", false, headers);

                //处理列表
                StorageQueue(commandQueueString2, 2);
                //存放队列
                Task.Delay(100);
                CommonHelper.ConsoleAndLogger($"{nameof(ObtainQueueJob)}=>获取列表完成... {DateTime.Now}", CommonHelper.LoggerType.Info);

            }
            catch
            {

            }
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