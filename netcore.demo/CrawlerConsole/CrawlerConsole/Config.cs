using Crawler.Models;
using Crawler.Service.Config;
using System;
using System.Collections.Generic;
using System.Text;
namespace CrawlerConsole
{
    public static class Config
    {
        public static readonly string localUrl = @"http://localhost:8088/";
        public static readonly string remoteUrl = @"https://unibone.dev.heywind.cn/";
        public static readonly string dynamicUrl = remoteUrl;
        public static bool iSLocalEnvironment = true;        //true代表 是本地环境 北京时间  false 8小时时差
        public static readonly string igUrl = "https://www.instagram.com";
        public static readonly string unUrl = dynamicUrl;
        public static readonly string jsonPars = "{\"Password\":\"12345678\",\"Email\":\"164910441@qq.com\"}";
        public static readonly string contentType = "application/json";


        public static readonly string createInstagramPostUrl = dynamicUrl + @"Tarpa/InstagramPosts/CreateInstagramPost";

        public static readonly string updateInstagramPostUrl = dynamicUrl + @"Tarpa/InstagramPosts/UpdateInstagramPost";
        public static readonly string updateInstagramPostUrl2 = dynamicUrl + @"Tarpa/InstagramPosts/UpdateInstagramPost";
        public static readonly string updateInstagramUserUrl = dynamicUrl+@"Tarpa/Kols/UpdateInstagramUser";


        public static readonly string uniboneTokenUrl = dynamicUrl+@"token";
        //public static readonly string uniboneTokenUrl = @"http://localhost:8088/token";
        public static readonly string commandQueueUrl = dynamicUrl + @"Tarpa/CommandQueues/GetCommandQueue";
        public static readonly string commandQueueListUrl = dynamicUrl + @"Tarpa/CommandQueues/GetCommandQueueList";
        //public static readonly string commandQueueListUrl = @"http://localhost:8088/Tarpa/CommandQueues/GetCommandQueueList";
        private static CookieInfoOptions _cookieInfoOptions;

        public static readonly string Cookie= ApplicationConfig.Configuration["Cookie"];
        public static CookieInfoOptions CookieInfoOptions => _cookieInfoOptions ?? (_cookieInfoOptions = new CookieInfoOptions
        {
            ig_did = ApplicationConfig.Configuration["CookieInfoOptions:ig_did"],
            mid = ApplicationConfig.Configuration["CookieInfoOptions:mid"],
            rur = ApplicationConfig.Configuration["CookieInfoOptions:rur"],
            csrftoken = ApplicationConfig.Configuration["CookieInfoOptions:csrftoken"],
            ds_user_id = ApplicationConfig.Configuration["CookieInfoOptions:ds_user_id"],
            urlgen = ApplicationConfig.Configuration["CookieInfoOptions:urlgen"],
            fbm_124024574287414 = ApplicationConfig.Configuration["CookieInfoOptions:fbm_124024574287414"],
            sessionid = ApplicationConfig.Configuration["CookieInfoOptions:sessionid"]

        });

        private static RabbitMQOption _rabbitMQOptions;

        public static RabbitMQOption RabbitMQOptions => _rabbitMQOptions ?? (_rabbitMQOptions = new RabbitMQOption
        {
            Exchange = ApplicationConfig.Configuration["RabbitMQ:Exchange"],
            Host = ApplicationConfig.Configuration["RabbitMQ:Host"],
            Port = int.Parse(ApplicationConfig.Configuration["RabbitMQ:Port"]),
            UserName = ApplicationConfig.Configuration["RabbitMQ:UserName"],
            PassWord = ApplicationConfig.Configuration["RabbitMQ:PassWord"],
            RoutingKey = ApplicationConfig.Configuration["RabbitMQ:RoutingKey"]
        });
    }

}