using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Signalr.Server
{
    public class SendStock : AbstractSender
    {
        private readonly SemaphoreSlim _marketStateLock = new SemaphoreSlim(1, 1);
        public SendStock(IHubContext<SenderHub, ISender> senderHub) : base(senderHub)
        {

        }

        public async override Task SendMessageAsync(string type)
        {
            await _marketStateLock.WaitAsync();
            try
            {
              
                await SendAsync("message");
            }
            finally
            {
                _marketStateLock.Release();
            }
        }

        public override ChannelReader<string> SendStreamAsync(string type,int delay,CancellationToken cancellationToken)
        {
           
            var channel = Channel.CreateUnbounded<string>();
            _ = WriteItemsAsync("message",channel.Writer, delay, cancellationToken);
            return channel.Reader;
        }
    }
}
