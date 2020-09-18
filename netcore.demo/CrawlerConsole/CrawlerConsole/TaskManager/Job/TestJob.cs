using CrawlerConsole.TaskManager.Job;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerConsole.Job
{

    public class TestMethod : CommandJob
    {
        static int index = 100;
        public async override Task Execute(IJobExecutionContext context)
        {
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            ExecuteAction(()=> {
                 Request(job_dataMap, trigger_dataMap);
            });
           
        }
        private void Request(JobDataMap job_dataMap, JobDataMap trigger_dataMap)
        {
            
            Console.WriteLine($"{index}start：{DateTime.Now} task1");
            //for (int i = 0; i < 1000; i++)
            //{
            //    int res = i;
               
            //   Task.Run(() =>
            //    {
            //        Console.WriteLine($"{index}测试的任务开始 {DateTime.Now}  {res}");
            //        Console.WriteLine($" {index}获取job参数为: {job_dataMap.Get("testJobDetail")}  {res}");
            //        Console.WriteLine($" {index}获取trigger参数为: {trigger_dataMap.Get("testTrigger")}  {res}");
            //        Thread.Sleep(10000);
            //        Console.WriteLine($"{index}测试的任务结束 {DateTime.Now} {res} ");
            //    });
                 
            //}
            List<int> lsit = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                lsit.Add(i);
            }
          
            Parallel.ForEach(lsit, (i) => {
                int res = i;
                Console.WriteLine($"{index}测试的任务开始 {DateTime.Now}  {res}");
                Console.WriteLine($" {index}获取job参数为: {job_dataMap.Get("testJobDetail")}  {res}");
                Console.WriteLine($" {index}获取trigger参数为: {trigger_dataMap.Get("testTrigger")}  {res}");
                Thread.Sleep(10000);
                Console.WriteLine($"{index}测试的任务结束 {DateTime.Now} {res} ");
            });


           
            Console.WriteLine($"{index}end：{DateTime.Now}");
            index++;
        }
    }

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
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class TestJob3 : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"start：{DateTime.Now} task3");
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            await Task.Run(() => {

                Console.WriteLine($"测试的任务开始 {DateTime.Now}");
                Console.WriteLine($" 获取job参数为: {job_dataMap.Get("testJobDetail")}");
                Console.WriteLine($" 获取trigger参数为: {trigger_dataMap.Get("testTrigger")}");
                Thread.Sleep(1000*60*2);
 
                Console.WriteLine($"测试的任务结束 {DateTime.Now}");
            });
            Console.WriteLine($"end：{DateTime.Now}");
        }
    }
}
