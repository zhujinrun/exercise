using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Signalr.Server
{
    public class SendOrder : AbstractSender
    {
        private readonly SemaphoreSlim _marketStateLock = new SemaphoreSlim(1, 1);
        public SendOrder(IHubContext<SenderHub, ISender> senderHub) : base(senderHub) { }

        public async override Task SendMessageAsync(string type)
        {
            await _marketStateLock.WaitAsync();
            try
            {
                //业务代码 ...
                //推送
                await SendAsync("待推送内容,string推送");
            }
            finally
            {
                _marketStateLock.Release();
            }
        }

        public override ChannelReader<string> SendStreamAsync(string type,int delay,CancellationToken cancellationToken)
        {
            //业务代码 ...
            //推送
            var channel = Channel.CreateUnbounded<string>();
            _ = WriteItemsAsync("待推送内容，stream形式传输", channel.Writer, delay, cancellationToken);
            return channel.Reader;
        }
    }
}
