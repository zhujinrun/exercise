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

namespace CrawlerConsole
{
    public class TestDi
    {
        public void Do()
        {
            Console.WriteLine("this is test");
        }
    }
    public class Program
    {
        /// <summary>
        /// 存放任务队列
        /// </summary>
        public static List<JData> jDatas = new List<JData>();
        static async Task Main(string[] args)
        {
            //获取 指令做判断
            {            
            }
            {
                //ITest test = ServiceDiExtension.GetService<ITest>();
                //test.Go();
                //Console.Read();
            }
            {
                //任务测试
                TaskSchedulers.jobDetail_Collection.Add("testJobDetail", "jobDetail1");
                TaskSchedulers.trigger_collection.Add("testTrigger", "jobTrigger1");
              //  await TaskSchedulers.Invoke<KolShortCodeJob>("5/10 * * * * ?", "shortcode", "kol分组", "获取shortcode");
               // await TaskSchedulers.Invoke<KolPostJob>("5/10 * * * * ?", "post", "kol分组", "获取post");
                await TaskSchedulers.Invoke<ObtainQueueJob>("5/10 * * * * ?", "obtainQueue", "queue分组", "获取队列指令");
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
