﻿using IDCM.Contract.IGrains;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;

namespace IDCM.Contract.Client.Extension
{

    public static partial class Extensions
    {
        public static IServiceCollection AddExtOrleansMultiClient(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddOrleansMultiClient(build =>
            {


                build.AddClient(opt =>
                {
                    opt.ServiceId = "foudation";
                    opt.ClusterId = "foudation";

                    opt.SetServiceAssembly(typeof(IBaseDataGrains).Assembly);
                    opt.Configure = (b =>
                    {
                        b.UseLocalhostClustering();
                        b.AddOutgoingGrainCallFilter<ExceptionCallFilter>();
                        //b.AddApplicationInsightsTelemetryConsumer("INSTRUMENTATION_KEY");
                    });
                });
            });
            return services;
        }
    }
}