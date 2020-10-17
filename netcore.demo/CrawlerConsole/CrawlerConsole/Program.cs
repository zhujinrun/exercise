using Crawler.Common;
using Crawler.Logger;
using Crawler.Models;
using Crawler.QuartzNet;
using Crawler.Service;
using Crawler.Utility.HttpHelper;
using CrawlerConsole.DiService;
using CrawlerConsole.TaskManager;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole
{
    public class Program
    {

        /// <summary>
        /// 存放任务队列
        /// </summary>
        public static HashSet<JData> jDatas = new HashSet<JData>();
        static async Task Main(string[] args)
        {
            if (!TryRegisterService())
            {
                throw new Exception("实例未初始化");
            }
            //环境判断
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Config.iSLocalEnvironment = true;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Config.iSLocalEnvironment = false;
            }
            //日志测试
            LoggerHelper.Info("start logger");
            //获取 指令做判断


            //TaskSchedulers.jobDetail_Collection.Add("testJobDetail", "jobDetail1");
            //TaskSchedulers.trigger_collection.Add("testTrigger", "jobTrigger1");
            //任务测试 0 0 0 * * ? *  每天零点执行一次  0 0/10 * * * ? *每十分钟执行一次   每一分钟执行一次0 0/1 * * * ? *

            await TaskSchedulers.Invoke<ObtainQueueJob>("", "obtainQueue", "queue分组", "获取队列指令");
            await Task.Delay(1000);

              //await TaskSchedulers.Invoke<KolShortCodeJob>("0 0/1 * * * ? *", "shortcode", "kol分组", "获取shortcode");
             await TaskSchedulers.Invoke<KolProfileJob>("0 0/1 * * * ? *", "profile", "kol分组", "获取Profile");
             await TaskSchedulers.Invoke<KolPostJob>("0 0/1 * * * ? *", "post", "kol分组", "获取Post");
            if (!Config.iSLocalEnvironment)
            {
                while (true)
                {
                    Thread.Sleep(1000 * 60 * 1);
                    LoggerHelper.Info($"working{CommonHelper.GetSTime()}");
                }

            }
            else
            {
                Console.ReadLine();
            }

        }
        private static bool TryRegisterService()
        {
            try
            {
                CustomApplicationService.RegisterServices(ServiceDiExtension.AddServer);
                CustomApplicationService.BuildServices();
                return true;
            }
            catch (Exception exception)
            {
                ConsoleHelper.ServerWriteError(exception);
                return false;
            }
        }
    }

}
