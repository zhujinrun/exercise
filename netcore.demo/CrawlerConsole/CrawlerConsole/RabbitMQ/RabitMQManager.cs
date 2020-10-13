using Crawler.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CrawlerConsole.RabbitMQ
{

    /*
     Test test = new Test();
            test.Age = 1;
            test.Name = "test110";
          
            RabbitMqManager._().Publish(opstions.Exchange, "Test", opstions.RoutingKey, JsonConvert.SerializeObject(test), false);
            RabbitMqManager._().Subscribe<Test>("Test", false, (a) => { }, false);
     */
    public class RabitMQManager
    {
        #region Fields
        private static RabitMQManager uniqueInstance;
        readonly IConnection connection;
        private static ConnectionFactory _connectionFactory;
        private static readonly object locker = new object();
        private static readonly object locker1 = new object();
        private static readonly ConcurrentDictionary<string, IModel> ChannelDic = new ConcurrentDictionary<string, IModel>();
        private static RabbitMQOption _options = Config.RabbitMQOptions;
        #endregion

        #region Constructors
        private RabitMQManager()
        {
            connection = ConnectionFactory.CreateConnection();
        }
        #endregion

        #region Utilities
        public static RabitMQManager _()
        {
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new RabitMQManager();
                    }
                }
            }
            return uniqueInstance;
            //return new RabbitMqGear();
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">队列信息</param>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名</param>
        /// <param name="isProperties">是否持久化</param>
        /// <returns></returns>
        public void Publish(string exchange, string queue, string routingKey, string body, bool isProperties = false)
        {
            var channel = GetModel(exchange, queue, routingKey, isProperties);

            try
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange, string.IsNullOrEmpty(exchange) ? queue : routingKey, properties, Encoding.UTF8.GetBytes(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <param name="handler">消费处理</param>
        /// <param name="isDeadLetter"></param>
        public void Subscribe<T>(string queue, bool isProperties, Action<T> handler, bool isDeadLetter) where T : class
        {
            //队列声明
            var channel = GetModel(queue, isProperties);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msgStr = Encoding.UTF8.GetString(body.ToArray());
                try
                {
                    T msg = JsonConvert.DeserializeObject<T>(msgStr);
                    handler(msg);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {

                    if (!isDeadLetter)
                    {
                        //    PublishToDead<DeadLetterQueue>(queue, msgStr, ex);
                    }
                    channel.BasicReject(ea.DeliveryTag, true);
                }
                finally
                {
                }
            };
            channel.BasicConsume(queue, false, consumer);
        }
        #endregion

        #region Methods
        private static ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    lock (locker1)
                    {
                        if (_connectionFactory == null)
                        {
                            var hostName = _options.Host;
                            var userName = _options.UserName;
                            var password = _options.PassWord;
                            //  var port = int.Parse(GlobalConfiguration.UniConfiguration.Get("Unibone.RabbitMq.Port")?.IsBlankThen("5672"));
                            _connectionFactory = new ConnectionFactory()
                            {
                                HostName = hostName,
                                UserName = userName,
                                Password = password,
                                //  Port = port
                            };
                        }
                    }
                }
                return _connectionFactory;
            }
        }

        /// <summary>
        /// 交换器声明
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchange">交换器</param>
        /// <param name="type">交换器类型：
        /// 1、Direct Exchange – 处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全
        /// 匹配。这是一个完整的匹配。如果一个队列绑定到该交换机上要求路由键 “dog”，则只有被标记为“dog”的
        /// 消息才被转发，不会转发dog.puppy，也不会转发dog.guard，只会转发dog
        /// 2、Fanout Exchange – 不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都
        /// 会被转发到与该交换机绑定的所有队列上。很像子网广播，每台子网内的主机都获得了一份复制的消息。Fanout
        /// 交换机转发消息是最快的。
        /// 3、Topic Exchange – 将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。符号“#”匹配一个或多
        /// 个词，符号“*”匹配不多不少一个词。因此“audit.#”能够匹配到“audit.irs.corporate”，但是“audit.*”
        /// 只会匹配到“audit.irs”。</param>
        /// <param name="durable">持久化</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        private void ExchangeDeclare(IModel channel, string exchange, string type = ExchangeType.Direct,
            bool durable = true,
            bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            exchange = string.IsNullOrWhiteSpace(exchange) ? "" : exchange.Trim();
            channel.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        /// <summary>
        /// 队列声明
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queue">队列</param>
        /// <param name="durable">持久化</param>
        /// <param name="exclusive">排他队列，如果一个队列被声明为排他队列，该队列仅对首次声明它的连接可见，
        /// 并在连接断开时自动删除。这里需要注意三点：其一，排他队列是基于连接可见的，同一连接的不同信道是可
        /// 以同时访问同一个连接创建的排他队列的。其二，“首次”，如果一个连接已经声明了一个排他队列，其他连
        /// 接是不允许建立同名的排他队列的，这个与普通队列不同。其三，即使该队列是持久化的，一旦连接关闭或者
        /// 客户端退出，该排他队列都会被自动删除的。这种队列适用于只限于一个客户端发送读取消息的应用场景。</param>
        /// <param name="autoDelete">自动删除</param>
        /// <param name="arguments">参数</param>
        private void QueueDeclare(IModel channel, string queue, bool durable = true, bool exclusive = false,
            bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            queue = string.IsNullOrWhiteSpace(queue) ? "UndefinedQueueName" : queue.Trim();
            channel.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey"></param>
        /// <param name="isProperties">是否持久化</param>
        /// <returns></returns>
        private IModel GetModel(string exchange, string queue, string routingKey, bool isProperties = false)
        {
            return ChannelDic.GetOrAdd(queue, key =>
            {
                var model = connection.CreateModel();
                //var properties = model.CreateBasicProperties();
                //properties.Persistent = true;
                QueueDeclare(model, queue, isProperties);

                if (!string.IsNullOrEmpty(exchange))
                {
                    ExchangeDeclare(model, exchange, ExchangeType.Fanout, isProperties);
                    model.QueueBind(queue, exchange, routingKey);
                }

                ChannelDic[queue] = model;
                return model;
            });
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="isProperties"></param>
        /// <returns></returns>
        private IModel GetModel(string queue, bool isProperties = false)
        {
            return ChannelDic.GetOrAdd(queue, value =>
            {
                var model = connection.CreateModel();
                QueueDeclare(model, queue, isProperties);

                //每次消费的消息数
                model.BasicQos(0, 1, false);

                ChannelDic[queue] = model;

                return model;
            });
        }
        #endregion
    }
}
