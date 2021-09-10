using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace k_Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = string.Empty;
            var config = new ConsumerConfig
            {
                BootstrapServers = "13.212.67.10:9082",
                GroupId = "001",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("test");
                result = consumer.Consume().Message.Value;
            }
            Console.WriteLine(result);
            await Task.CompletedTask;
        }
    }
    
}
