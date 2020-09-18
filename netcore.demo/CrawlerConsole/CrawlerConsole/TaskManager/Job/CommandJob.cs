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
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{
   [PersistJobDataAfterExecution]
   [DisallowConcurrentExecution]
    public abstract class CommandJob : ICommandJob<JData>
    {
        private static string tokenString;

        static CommandJob()
        {
            if (string.IsNullOrWhiteSpace(tokenString))
            {
                TokenHelper helper = ServiceDiExtension.GetService<TokenHelper>();
                
                tokenString = helper.GetToken(Config.uniboneTokenUrl, "application/json", Config.jsonPars);
                if (string.IsNullOrWhiteSpace(tokenString))
                    throw new Exception("获取token失败");
            }
        }
        public string TokenString
        {
            set
            {
                tokenString = value;
            }
            get
            {
                return tokenString;
            }
        }

        public abstract Task Execute(IJobExecutionContext context);
        
        /// <summary>
        /// 任务执行方法
        /// </summary>
        /// <param name="action">动作</param>
        /// <param name="lcount">根据实际需要设置循环次数</param>
        /// <returns></returns>
        public virtual void ExecuteAction(Action action)
        {
            action();
        }
        public IList<JData> GetCommList(string actionType)
        {
            return Program.jDatas.Where(x => x.action.ToLower().Equals(actionType)).OrderByDescending(x => x.level).ToList();
        }

    }
}
