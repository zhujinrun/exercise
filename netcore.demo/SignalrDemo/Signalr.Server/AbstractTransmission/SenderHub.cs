using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Signalr.Server
{

    #region MyRegion
    public class SenderHub : Hub<ISender>
    {
        private readonly AbstractSender _sender;
        public SenderHub(AbstractSender sender)
        {
            _sender = sender;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message)
        {
            await _sender.SendMessageAsync(message);
        }
        /// <summary>
        /// 流式传输
        /// </summary>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ChannelReader<string> SendStreamAsync(string message,int delay,CancellationToken cancellationToken)
        {
            return _sender.SendStreamAsync(message,delay, cancellationToken);
        }
    }
    #endregion
}
