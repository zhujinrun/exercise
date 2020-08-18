using Crawler.Common.ExtensionsAttribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Common.Model
{
    public class SeleCookie
    {
        /*
        ig_did=1E73D3DB-173D-4E50-BABB-4A69935242C3; 
        mid=XxajXgALAAH94QzjWotyscbXSy2a; 
        rur=FRC; 
        csrftoken=RTy6DxCGXDSacnFpfSemcbJuSmqx8UEk; 
        ds_user_id=39391854053; 
        urlgen="{\"18.162.125.250\": 16509}:1jxnOT:9YlK7uxa_MDEOXm7onEGiia-ib8"; 
        sessionid=39391854053%3AkxrdjF6zGdFnej%3A25
         */
        [Custom]
        public string ig_did { get; set; }
        [Custom]
        public string mid { get; set; }
        [Custom]
        public string rur { get; set; }
        [Custom]
        public string csrftoken { get; set; }
        [Custom]
        public string ds_user_id { get; set; }
        [Custom]
        public string urlgen { get; set; }
        [Custom]
        public string sessionid { get; set; }


        public string this[string name]
        {
            get
            {
                switch (name)
                {

                    case "ig_did":
                        return ig_did;
                    case "mid":
                        return mid;
                    case "rur":
                        return rur;
                    case "csrftoken":
                        return csrftoken;
                    case "ds_user_id":
                        return ds_user_id;
                    case "urlgen":
                        return urlgen;
                    case "sessionid":
                        return sessionid;
                    default:
                        throw new Exception($"未能获取-{name}-的值");

                }

            }
        }

    }
}
