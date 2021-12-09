using Confluent.Kafka;
using System;
using System.Net;
using System.Threading.Tasks;

namespace k_Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig
            {
                //BootstrapServers = "13.212.67.10:9082",
                BootstrapServers = "8.142.71.127:9092",

                ClientId = Dns.GetHostName()
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                //await producer.ProduceAsync("test", new Message<Null, string> { Value = "a log message" });

                var t = producer.ProduceAsync("test", new Message<Null, string> { Value = "hello world" });
                await t.ContinueWith(async task =>
               {
                   if (task.IsFaulted)
                   {
                       await Task.CompletedTask;
                   }
                   else
                   {
                       await Console.Out.WriteAsync($"Wrote to offset: {task.Result.Offset}");
                   }
               });
            }
            Console.Read();
        }
    }
}
