using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.API.Job
{

    [PersistJobDataAfterExecution]//执行后保留数据,更新JobDataMap
    [DisallowConcurrentExecution]//拒绝同一时间重复执行，同一任务串行
    public class TestJob : IJob  
    {
        public TestJob()
        {
            Console.WriteLine("testjob 构造函数");
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap job_dataMap = context.JobDetail.JobDataMap;
            JobDataMap trigger_dataMap = context.Trigger.JobDataMap;
            await Task.Run(()=> {
                
                Console.WriteLine("测试的任务开始");
                Console.WriteLine($" 获取trigger参数为: {trigger_dataMap.Get("testTrigger")}");
                Console.WriteLine($" 获取job参数为: {job_dataMap.Get("testJobDetail")}");
                Thread.Sleep(1000);
                Console.WriteLine("测试的任务结束");
            });
            
        }
    }
}
