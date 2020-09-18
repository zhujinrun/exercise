using Crawler.Logger;
using Crawler.Models;
using Crawler.QuartzNet;
using Crawler.Service;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.Job;
using CrawlerConsole.TaskManager.Job;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace CrawlerConsole { 
    public class Program
    {
        /// <summary>
        /// 存放任务队列
        /// </summary>
        public static HashSet<JData> jDatas = new HashSet<JData>();
        static async Task Main(string[] args)
        {
            //日志测试
            LoggerHelper.Info("start logger");
            //获取 指令做判断
            {
                TaskSchedulers.jobDetail_Collection.Add("testJobDetail", "jobDetail1");
                TaskSchedulers.trigger_collection.Add("testTrigger", "jobTrigger1");

                //任务测试 0 0 0 * * ? *  每天零点执行一次  0 0/10 * * * ? *每十分钟执行一次   每一分钟执行一次0 0/1 * * * ? *

                 await TaskSchedulers.Invoke<ObtainQueueJob>("", "obtainQueue", "queue分组", "获取队列指令");
                 await Task.Delay(1000);
                //await TaskSchedulers.Invoke<KolProfileJob>("0 0/1 * * * ? *", "profile", "kol分组", "获取Profile");
                await TaskSchedulers.Invoke<KolPostJob>("0 0/1 * * * ? *", "post", "kol分组", "获取Post");
                //await TaskSchedulers.Invoke<KolShortCodeJob>("0 0/1 * * * ? *", "shortcode", "kol分组", "获取shortcode");

                //await TaskSchedulers.Invoke<TestJob3>("0/30 * * * * ? *", "queue分组", "获取队列指令"); //执行一次
                //await TaskSchedulers.Invoke<TestMethod>("0/1 * * * * ? *", "queue分组2", "获取队列指令2"); //执行一次
                //await TaskSchedulers.Invoke<TestJob>("0 0/1 * * * ? *", "queue分组3", "获取队列指令3"); //执行一次
            }

            Console.Read();
        }

    }

}
