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
        public static List<JData> jDatas = new List<JData>();
        static async Task Main(string[] args)
        {
            //日志测试
            LoggerHelper.Info("test logger");
            //获取 指令做判断
            {
                TaskSchedulers.jobDetail_Collection.Add("testJobDetail", "jobDetail1");
                TaskSchedulers.trigger_collection.Add("testTrigger", "jobTrigger1");

                //任务测试

                await TaskSchedulers.Invoke<ObtainQueueJob>("", "obtainQueue", "queue分组", "获取队列指令");
                await Task.Delay(5000);
                await TaskSchedulers.Invoke<KolPostJob>("6/10 * * * * ?", "post", "kol分组", "获取post");

                await TaskSchedulers.Invoke<KolShortCodeJob>("5/10 * * * * ?", "shortcode", "kol分组", "获取shortcode");

                //await TaskSchedulers.Invoke<TestJob2>("6/10 * * * * ?", "queue分组", "获取队列指令"); //执行一次
                //await TaskSchedulers.Invoke<TestJob3>("6/10 * * * * ?", "queue分组2", "获取队列指令2"); //执行一次
                //await TaskSchedulers.Invoke<TestJob>("6/10 * * * * ?", "queue分组3", "获取队列指令3"); //执行一次
            }

            //捕获Ctrl+C事件  
            Console.CancelKeyPress += Console_CancelKeyPress;

            //进程退出事件  
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            //卸载事件  
            AssemblyLoadContext.Default.Unloading += Default_Unloading;
            Console.Read();
        }

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            
            if(KolShortCodeJob.driver!=null) KolShortCodeJob.driver.Quit() ;
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (KolShortCodeJob.driver != null) KolShortCodeJob.driver.Quit();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (KolShortCodeJob.driver != null) KolShortCodeJob.driver.Quit();
        }
    }

}
