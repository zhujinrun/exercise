using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signalr.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddSignalR()
                       .AddMessagePackProtocol();
            services.AddSingleton<AbstractSender,SendOrder>();
            services.AddSingleton<AbstractSender, SendStock>();    //以最后一个注册的为主，只能同时存在一个

        }
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SenderHub>("/senders");

            });
        }
    }
}
