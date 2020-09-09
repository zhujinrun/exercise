using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerConsole.TaskManager.Job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class KolProfileJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
