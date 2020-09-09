using Crawler.Models;
using Crawler.Service.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerConsole
{
    public static class Config
    {
        public static readonly string igUrl = "https://www.instagram.com/";
        //public static readonly string tokenUrl = @"https://unibone.dev.heywind.cn/token";
        public static readonly string jsonPars = "{\"Password\":\"12345678\",\"Email\":\"164910441@qq.com\"}";
        public static readonly string contentType = "application/json";


        public static readonly string createInstagramPostUrl = @"https://unibone.dev.heywind.cn/Tarpa/InstagramPosts/CreateInstagramPost";
        public static readonly string updateInstagramPostUrl = @"https://unibone.dev.heywind.cn/Tarpa/InstagramPosts/UpdateInstagramPost";
        public static readonly string updateInstagramUserUrl = @"https://unibone.dev.heywind.cn/Tarpa/Kols/UpdateInstagramUser";


        public static readonly string uniboneTokenUrl = @"https://unibone.dev.heywind.cn/token";
        public static readonly string commandQueueUrl = @"https://unibone.dev.heywind.cn/Tarpa/CommandQueues/GetCommandQueue";
        public static readonly string commandQueueListUrl = @"https://unibone.dev.heywind.cn/Tarpa/CommandQueues/GetCommandQueueList";
        private static CookieInfoOptions _cookieInfoOptions;

        public static readonly string Cookie= ApplicationConfig.Configuration["Cookie"];
        public static readonly string Cookie2= ApplicationConfig.Configuration["Cookie2"];
        public static readonly string Cookie3 = ApplicationConfig.Configuration["Cookie3"];
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
    }

}