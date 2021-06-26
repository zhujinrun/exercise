using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTool.Service.Impl
{
    public class ExceptionLessLogger : ILogger
    {
        public void Debug(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Debug).AddTags(tags).Submit();
        }

        public void Error(string message, params string[] args)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Error).AddTags(args).Submit();
        }

        public void Info(string message, params string[] args)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Info).AddTags(args).Submit();
        }

        public void Trace(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Trace).AddTags(tags).Submit();
        }

        public void Warn(string message, params string[] args)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Warn).AddTags(args).Submit();
        }
    }
}
