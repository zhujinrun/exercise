using Crawler.Models;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.token;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class ObtainQueueJob : IJob
    {
        private static string TokenString = string.Empty;
        public async Task Execute(IJobExecutionContext context)
        {
            if (string.IsNullOrWhiteSpace(TokenString))
            {
                TokenHelper helper = ServiceDiExtension.GetService<TokenHelper>();
                TokenString = helper.GetToken(Config.uniboneTokenUrl, "application/json", Config.jsonPars);
            }

            //获取队列列表

            IDictionary<string, string> headers = new Dictionary<string, string>
                {
                   {"Authorization","Bearer "+TokenString }
                };
            IDictionary<string, object> parmeters = new Dictionary<string, object>
                {
                   {"Level",10},
                   { "Action",null},
                   { "pageIndex",1},
                   { "pageSize",5}
                };

            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();
            string commandQueueString = webUtils.DoPost(Config.commandQueueUrl, null, "application/json", "", false, headers);
            string commandQueueString2 = webUtils.DoPost(Config.commandQueueListUrl, null, "application/json", JsonConvert.SerializeObject(parmeters), false, headers);

            //处理列表

            StorageQueue(commandQueueString);
            //存放队列       
            await Task.Delay(1);
        }

        /// <summary>
        /// 获取列表或者单条数据
        /// </summary>
        /// <param name="commandQueueString"></param>
        private static void StorageQueue(string commandQueueString)
        {
            List<JData> list = new List<JData>();
            ResponseMessage responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(commandQueueString);
            //列表 单条做处理
            JData jData = JsonConvert.DeserializeObject<JData>(JsonConvert.SerializeObject(responseMessage.data));
            List<JData> jDatas = JsonConvert.DeserializeObject<List<JData>>(JsonConvert.SerializeObject(responseMessage.data));
            CookieInfoOptions cio = JsonConvert.DeserializeObject<CookieInfoOptions>(jData.cookie);
            if (jData != null)
            {
                Program.jDatas.Add(jData);
            }
            if(jDatas.Count > 0)
            {
                Program.jDatas.AddRange(jDatas);
            }
            
        }
    }
}