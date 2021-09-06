using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Signalr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Signalr.Server
{
    public abstract class AbstractSender
    {

        private readonly IHubContext<SenderHub, ISender> SenderHub;
        public AbstractSender(IHubContext<SenderHub, ISender> senderHub)
        {
            SenderHub = senderHub;
        }

        public abstract Task SendMessageAsync(string type);     
        public abstract ChannelReader<string> SendStreamAsync(string type,int delay,CancellationToken cancellationToken);

        protected async Task WriteItemsAsync(string message, ChannelWriter<string> writer, int delay, CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                await writer.WriteAsync($"{message}", cancellationToken);
                await Task.Delay(delay, cancellationToken);
            }
            catch (Exception ex)
            {
                localException = ex;
            }
            finally
            {
                writer.Complete(localException);
            }
        }

        protected async Task SendAsync(string message)
        {
            await SenderHub.Clients.All.SendMessageAsync(message);
        }
    }

    #region MyRegion
    //public class Sender
    //{
    //    private readonly SemaphoreSlim _marketStateLock = new SemaphoreSlim(1, 1);
    //    private IHubContext<SenderHub, ISender> SenderHub { get; set; }
    //    public Sender(IHubContext<SenderHub, ISender> senderHub)
    //    {
    //        SenderHub = senderHub;
    //    }
    //    public Sender()
    //    {

    //    }
    //    public async Task SendMessageAsync(string message)
    //    {
    //        await _marketStateLock.WaitAsync();
    //        try
    //        {
    //            await SendAsync(message);
    //        }
    //        finally
    //        {
    //            _marketStateLock.Release();
    //        }
    //    }

    //    /// <summary>
    //    /// Stream传输
    //    /// </summary>
    //    /// <param name="count"></param>
    //    /// <param name="delay"></param>
    //    /// <param name="cancellationToken"></param>
    //    /// <returns></returns>
    //    public ChannelReader<string> SendStreamAsync(string message, int delay, CancellationToken cancellationToken)
    //    {
    //        var channel = Channel.CreateUnbounded<string>();
    //        _ = WriteItemsAsync(message, channel.Writer, delay, cancellationToken);
    //        return channel.Reader;
    //    }

    //    async Task WriteItemsAsync(string message, ChannelWriter<string> writer, int delay, CancellationToken cancellationToken)
    //    {
    //        Exception localException = null;
    //        try
    //        {
    //            await writer.WriteAsync($"{message}", cancellationToken);
    //            await Task.Delay(delay, cancellationToken);
    //        }
    //        catch (Exception ex)
    //        {
    //            localException = ex;
    //        }
    //        finally
    //        {
    //            writer.Complete(localException);
    //        }
    //    }

    //    async Task SendAsync(object message)
    //    {
    //        await SenderHub.Clients.All.SendMessageAsync(message);
    //    }
    //} 
    #endregion
}
