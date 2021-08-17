using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Signalr.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region 直接调用
            //var connection = new HubConnectionBuilder()
            //    .WithUrl($"http://localhost:9999/senders")
            //    .ConfigureLogging(logging =>
            //    {
            //        logging.AddConsole();
            //    })
            //    .AddMessagePackProtocol()
            //    .Build();

            //await connection.StartAsync();


            //#region string传输
            //connection.On<string>("SendMessageAsync", (message) =>
            //{
            //    Console.WriteLine($"opened,message = {message}");  //接受市场
            //});
            //await connection.InvokeAsync("SendMessageAsync", "1");
            //#endregion


            //#region stream传输
            //var cancellationTokenSource = new CancellationTokenSource();
            //var stream = connection.StreamAsync<string>(
            //    "SendStreamAsync", "ContractType", 500, cancellationTokenSource.Token);

            //await foreach (var count in stream)
            //{
            //    Console.WriteLine($"{count}");
            //}
            //#endregion
            //Console.WriteLine("Streaming completed"); 
            #endregion

            #region 实例化调用
            SignalReceiver signalReceiver = new("http://localhost:9999/senders");
            var cancellationTokenSource = new CancellationTokenSource();
            var result =await signalReceiver.ReveiveAsync(MethodType.SendMessageAsync, "1");
            Console.WriteLine($"result={result}");
            var result2 =await signalReceiver.ReveiveAsync(MethodType.SendStreamAsync, "ContractType", 500, cancellationTokenSource.Token);
            Console.WriteLine($"result2={JsonConvert.SerializeObject(result2)}"); 
            #endregion
            Console.Read();
        }
    }

  
}
