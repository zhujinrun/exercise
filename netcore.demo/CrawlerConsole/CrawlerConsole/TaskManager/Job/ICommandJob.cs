using Crawler.Utility.HttpHelper;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{
    public interface ICommandJob<T> :IJob where T:class 
    {
        /// <summary>
        /// 获取token
        /// </summary>
        string TokenString { get; set; }
        /// <summary>
        /// 获取属于自己指令列表
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IList<T> GetCommList(string action);
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task ExecuteAction(Action action, int lcount);
    }
}
