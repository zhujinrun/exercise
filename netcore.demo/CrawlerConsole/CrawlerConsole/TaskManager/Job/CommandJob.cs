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
   
    public abstract class CommandJob : ICommandJob<JData>
    {
        private static string tokenString;

        static CommandJob()
        {
            if (string.IsNullOrWhiteSpace(tokenString))
            {
                TokenHelper helper = ServiceDiExtension.GetService<TokenHelper>();
                tokenString = helper.GetToken(Config.uniboneTokenUrl, "application/json", Config.jsonPars);
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
        
        public async Task ExecuteAction(Action action, int lcount)
        {
            await Task.Delay(100);
            WebUtils webUtils = ServiceDiExtension.GetService<WebUtils>();

            List<Task> taskLists = new List<Task>();
            for (int i = 0; i < lcount; i++)
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
                        action();
                    }));
                }
            }
        }
        public IList<JData> GetCommList(string actionType)
        {
            return Program.jDatas.Where(x => x.action.ToLower().Equals(actionType)).OrderByDescending(x => x.level).ToList();
        }

    }
}
