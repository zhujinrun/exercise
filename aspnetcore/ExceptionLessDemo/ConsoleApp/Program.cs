using Exceptionless;
using System;

namespace ConsoleApp
{
    class Program
    {
        //https://be.exceptionless.io 账号密码 账号 victor405 邮箱164910441@qq.com 密码164910441@qq.com 

        static void Main(string[] args)
        {
            ExceptionlessClient.Default.Startup("rpWtOAEEpnYeBTbt3m0DtOiJ1qc5BN5PTIKG70Sk");
            //ExceptionlessClient.Default.Configuration.ServerUrl = "https://be.exceptionless.io"; 官网的不能这样写,会写不进去日志,自己搭建的可以。
            //ExceptionlessClient.Default.Configuration.ServerUrl = "https://52.149.199.118";     这样也不行
            ExceptionlessClient.Default.SubmitLog("这是一个普通日志记录code:{1-9}");
            try
            {
                throw new Exception($"异常此处抛出 时间=>> {DateTime.Now}");
            }catch(Exception ex)
            {
                ex.ToExceptionless().Submit();
            }
            Console.WriteLine("the end");
            Console.Read();

        }
    }
}
