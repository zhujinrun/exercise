using NC.MessageBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC.WebApi.MessageBus
{
    public class MessageBusTestHandler : AbsMessageHandler<Message<string>>
    {
        public async override Task OnHandle(Message<string> message, string path)
        {
            Console.WriteLine($"Message Handler TopicName:{path}, Time:{message.CreationDate}, Data:{message.Data}");
            await Task.CompletedTask;
        }
        /*
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:58 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:17:59, Data:2021/7/9 14:17:59 Producer Test publish.
        Message Handler TopicName:TopicTestName, Time:2021/7/9 14:18:00, Data:2021/7/9 14:17:59 Producer Test publish.
         */
    }
}
