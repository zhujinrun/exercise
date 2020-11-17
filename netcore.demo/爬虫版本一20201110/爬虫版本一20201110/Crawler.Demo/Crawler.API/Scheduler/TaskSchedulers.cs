using Crawler.API.Job;
using Crawler.API.Services;
using Crawler.Utility.HttpHelper;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unibone.Scheduler;

namespace Crawler.API.Scheduler
{
    public class TaskSchedulers
    {
        public static  PostDataService _postDataService;

        public static  WebUtils _webUtils;

        public static string Token = string.Empty;
        public async Task CraKolPost(string cookie, string cookie2,string url, PostDataService pdService, WebUtils webUtils)
        {
            _postDataService = pdService;
            _webUtils = webUtils;
            Console.WriteLine("任务开始!");
            QuartzService quartzService = new QuartzService();

            UniJob uniJob = new UniJob()
            {
                JobName = "testJob001",
                JobGroup = "testGroup001",
                TriggerName = "testTrigger001",
                TriggerGroup = "testGroup001",
                TriggerType = TriggerTypeEnum.Cron,
                CronExpression = "5/10 * * * * ?"   //  5/10 * * * * ?

            };

           // string readFromTxt = File.ReadAllText(@"D:\Heywind\shortcode.txt");
           // 特殊处理  
           string readFromTxt = File.ReadAllText(@"D:\Heywind\profile.txt");
            List<string> scList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(readFromTxt);     //获取shortcode  
            /*
             ig_did=1209A731-1052-47D1-A74E-A2FB067EA587; mid=X0TD9AALAAF5olUdhbis3cp8qTt8; fbm_124024574287414=base_domain=.instagram.com; csrftoken=ARNXGDiK5kQi1TTcRFHIQOTuhbeSNIws; ds_user_id=39391854053; sessionid=39391854053%3Agb9RnjJLkMEEHl%3A28; rur=FRC; urlgen="{\"45.78.34.12\": 25820}:1kAXLF:athjxrlGGHZUMRQfWzdTjRTT5Co"
             */
            var contentType = "application/json; charset=utf-8";
           
            var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Url = url,
                ShortCodeList = scList,
                Cookie = cookie,
                ContentType = contentType,
                PostDataService = pdService,
                Cookie2 = cookie2
            });
            if (!QuartzService.jobDetail_Collection.ContainsKey("testJobDetail"))
            {
                QuartzService.jobDetail_Collection.Add("testJobDetail", parameters);
            }

            await quartzService.AddJobAsync<GetKolPost>(uniJob);

            await UniSchedulerManager.Start();
        }
    }
}
