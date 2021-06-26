using Exceptionless;
using System;

namespace ConsoleApp
{
    class Program
    {
        //https://be.exceptionless.io 账号密码 账号 victor405 邮箱164910441@qq.com 密码164910441@qq.com 
        static void Main(string[] args)
        {
            ExceptionlessClient.Default.Configuration.ApiKey = "bA3NAoXc2OoYDBU18U4oZMITsAZJ4D45mE6KpvjI";
            var client = new ExceptionlessClient(ExceptionlessClient.Default.Configuration.ApiKey);
            
            ExceptionlessClient.Default.Configuration.ServerUrl = "https://be.exceptionless.io";
            ExceptionlessClient.Default.Startup(ExceptionlessClient.Default.Configuration.ApiKey);
            ExceptionlessClient.Default.SubmitLog("这是一个普通日志记录code:{123456789}");
            try
            {
                ExceptionlessClient.Default.CreateLog("出错了", Exceptionless.Logging.LogLevel.Error).Submit();
                throw new Exception($"异常此处抛出 时间 {DateTime.Now}");
            }catch(Exception ex)
            {
                client.SubmitException(ex);
                //ex.ToExceptionless().Submit();
            }
            Console.WriteLine("the end");
            Console.Read();

        }
    }
}
