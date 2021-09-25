using IDCM.Contract.BarWebApi.Orleans;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDCM.Contract.Core.Extension;

namespace IDCM.Contract.BarWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseExtOrleans(builder =>
                {
                    builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FoundationGrains).Assembly).WithReferences())
                    .AddIncomingGrainCallFilter<ExceptionCallFilter>();
                    ;

                });
    }
}
