using LockService.Helper;
using LockService.Business;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LockService
{
    class Program
    {
        //需开启多个控制台一起跑测试
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddCommandLine(args);
            var configuration = builder.Build();

            int minute = int.Parse(configuration["minute"]);   //设置开始秒杀时间   
            using (var client = new ConnectionHelper().Conn())
            {
                //设置库存10
                client.Set<int>("inventoryNum", 10);
                //订单  设置订单10
                client.Set<int>("orderNum", 10);
                Console.WriteLine($"在{minute}分0秒正式开始秒杀! ");
                var flag = true;
                while (flag)
                {
                    if (DateTime.Now.Minute == minute)
                    {
                        flag = false;

                        Parallel.For(0, 30, (i) => {

                            int temp = i;

                             //NormalSecondsKill.Show(); //lock锁  会出现超卖情况 。 原因是非同一个线程锁不住

                            BlockingLock.Show(i, "akey", TimeSpan.FromSeconds(100));   //阻塞锁 ，可防止超卖，速度比不上非阻塞锁


                            //ImmediatelyLock.Show(i, "akey", TimeSpan.FromSeconds(100));   //非阻塞锁 ，会出现卖不完情况
                        });
                        Thread.Sleep(100);
                    }
                }

            }
            Console.ReadKey();
        }
    }
}
