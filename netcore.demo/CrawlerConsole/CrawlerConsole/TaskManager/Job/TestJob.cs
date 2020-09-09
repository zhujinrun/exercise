using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole.Job
{
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"start：{DateTime.Now} task1");
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            await Task.Run(() => {

                Console.WriteLine("测试的任务开始");
                Console.WriteLine($" 获取job参数为: {job_dataMap.Get("testJobDetail")}");
                Console.WriteLine($" 获取trigger参数为: {trigger_dataMap.Get("testTrigger")}");
                Thread.Sleep(1000);
                Console.WriteLine("测试的任务结束");
            });
            Console.WriteLine($"end：{DateTime.Now}");
        }
    }

    public class TestJob2 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"start：{DateTime.Now} task2");
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            await Task.Run(() => {

                Console.WriteLine("测试的任务开始");
                Console.WriteLine($" 获取job参数为: {job_dataMap.Get("testJobDetail")}");
                Console.WriteLine($" 获取trigger参数为: {trigger_dataMap.Get("testTrigger")}");
                Thread.Sleep(1000);
                Console.WriteLine("测试的任务结束");
            });
            Console.WriteLine($"end：{DateTime.Now}");
        }
    }

    public class TestJob3 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"start：{DateTime.Now} task3");
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            await Task.Run(() => {

                Console.WriteLine("测试的任务开始");
                Console.WriteLine($" 获取job参数为: {job_dataMap.Get("testJobDetail")}");
                Console.WriteLine($" 获取trigger参数为: {trigger_dataMap.Get("testTrigger")}");
                Thread.Sleep(1000);
                Console.WriteLine("测试的任务结束");
            });
            Console.WriteLine($"end：{DateTime.Now}");
        }
    }
}
