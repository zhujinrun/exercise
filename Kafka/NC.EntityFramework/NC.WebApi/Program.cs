using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NC.MessageBus;
using NC.MessageBus.Abstractions;
using NC.MessageBus.DependencyInjection;
using NC.WebApi.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseMessageBus(() => new List<IProducer>() { new Producer<Message<string>>("TopicTestName") },()=> new List<IConsumer>() { new Consumer<Message<string>, MessageBusTestHandler>("TopicTestName") })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
