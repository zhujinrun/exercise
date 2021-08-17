using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Signalr.Client
{
    public class SignalReceiver
    {
        private HubConnection _connection;

        public SignalReceiver(string url)
        {
            Init(url).GetAwaiter().GetResult();
        }

        public async Task Init(string url)
        {
            _connection = new HubConnectionBuilder()
             .WithUrl(url)
             .ConfigureLogging(logging =>
             {
                 logging.AddConsole();
             })
             .AddMessagePackProtocol()
             .Build();
            if (_connection.State == HubConnectionState.Disconnected)
                await _connection.StartAsync();
        }

        private async Task<string> ReveiveMessageAsync(string methodName, string type, int time, CancellationToken cancellationToken)
        {
            string result = string.Empty;
            _connection.On<string>(methodName, (message) =>
            {
                result = message;
            });
            await _connection.InvokeAsync(methodName, type);
            return result;
        }

        private async Task<List<string>> ReveiveStreamAsync(string methodName, string type, int time, CancellationToken cancellationToken)
        {
            List<string> result = new List<String>();
            var stream = _connection.StreamAsync<string>(methodName, type, time, cancellationToken);
            await foreach (var item in stream)
            {
                result.Add(item);
            }
            return result;
        }

        public async Task<object> ReveiveAsync(MethodType methodName, string type, int time = 500, CancellationToken cancellationToken = default(CancellationToken))
        {
            object result = null;
            switch (methodName)
            {
                case MethodType.SendMessageAsync:
                    result = await ReveiveMessageAsync(methodName.ToString(), type, time, cancellationToken);
                    break;

                case MethodType.SendStreamAsync:
                    result = await ReveiveStreamAsync(methodName.ToString(), type, time, cancellationToken);
                    break;

                default:
                    throw new NotSupportedException("未知");
            }
            return result;
        }
    }
    public enum MethodType
    {
        SendStreamAsync,
        SendMessageAsync
    }
}
