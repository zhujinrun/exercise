
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NC.MessageBus.Hosting;
using NC.MessageBus.Options;
using NC.MessageBus.Utility;
using SlimMessageBus.Host.Config;
using SlimMessageBus.Host.Kafka;
using SlimMessageBus.Host.Kafka.Configs;
using SlimMessageBus.Host.MsDependencyInjection;
using SlimMessageBus.Host.Serialization.Json;
using System;
using System.Collections.Generic;

namespace NC.MessageBus.DependencyInjection
{
    /// <summary>
    /// HostBuilder 扩展
    /// </summary>
    public static class DIExtensions
    {
        public static IHostBuilder UseMessageBus(this IHostBuilder hostBuilder,Func<IEnumerable<IProducer>> addProducer=null,Func<IEnumerable<IConsumer>> addConsumer=null)
        {
            hostBuilder.ConfigureServices((host, services) =>
            {
                var configuration = host.Configuration;
                var options = configuration.GetSection("messagebus").Get<MessageBusOptions>();
                if (options == null)
                {
                    throw new Exception("消息总线初始化异常：缺少配置信息");
                }
                foreach (var consumer in addConsumer?.Invoke())
                {
                    services.AddTransient(consumer.HandlerType);
                }
                services.AddHostedService<MessageBusBootstraper>();

                MessageBusBootstraper.Bootstarp += serviceProvier => SlimMessageBus.MessageBus.SetProvider(() => serviceProvier.GetService<SlimMessageBus.IMessageBus>());
                services.AddSingleton(svp => BuildMessageBus(svp,options,addProducer?.Invoke(),addConsumer?.Invoke()));
                services.AddSingleton<NC.MessageBus.Abstractions.IMessageBus>(svp => new NC.MessageBus.Abstractions.MessageBus());
            });
            return hostBuilder;
        }

         static SlimMessageBus.IMessageBus BuildMessageBus(IServiceProvider serviceProvider,MessageBusOptions options,IEnumerable<IProducer> producers,IEnumerable<IConsumer> consumers)
        {
            void AddSsl(ClientConfig c)
            {
                //c.SecurityProtocol = SecurityProtocol.SaslSsl;
                //c.SaslUsername = options.Username;
                //c.SaslPassword = options.Password;
                //c.SaslMechanism = SaslMechanism.ScramSha256;
                //c.SslCaLocation = "cloudkarafka_2020-12.ca";
            }

            var mbb = MessageBusBuilder.Create()
                .Do(builder =>
                {
                    producers?.ForEach(f => builder.Produce(f.MessageType, x => x.DefaultTopic(f.TopicName)));
                    consumers?.ForEach(f => builder.Consume(f.MessageType, x => x.Topic(f.TopicName).WithConsumer(f.HandlerType).Group(f.Group)));
                    builder.WithSerializer(new JsonMessageSerializer());
                    builder.WithDependencyResolver(new MsDependencyInjectionDependencyResolver(serviceProvider));
                    builder.WithProviderKafka(new KafkaMessageBusSettings(options.Brokers)
                    {
                        ProducerConfig = (config) =>
                        {
                            AddSsl(config);
                        },
                        ConsumerConfig = (config) =>
                        {
                            config.AutoOffsetReset = AutoOffsetReset.Latest;
                            AddSsl(config);
                        }
                    });
                }).Build();

            return mbb;

        }
    }
}
