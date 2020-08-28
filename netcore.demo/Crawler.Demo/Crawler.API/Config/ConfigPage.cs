using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.API.Config
{
    public static class ConfigPage
    {
        public static readonly string tokenUrl = @"http://localhost:8088/token";
        public static readonly string jsonPars = "{\"Password\":\"12345678\",\"Email\":\"164910441@qq.com\"}";
        public static readonly string contentType = "application/json";


        public static readonly string createInstagramPostUrl = @"http://localhost:8088/Tarpa/InstagramPosts/CreateInstagramPost";
        public static readonly string updateInstagramPostUrl = @"http://localhost:8088/Tarpa/InstagramPosts/UpdateInstagramPost";
        
    }
}
