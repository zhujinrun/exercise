using Crawler.Common;
using Crawler.Common.Model;
using Crawler.Common.Selenuim;
using Crawler.Http.Utility;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace NUnitTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // httpgetÇëÇó
            HttpGetTest();
            //µÇÂ¼²âÊÔ
            SeleLogin();
        }

        public void SeleLogin(string url= "https://www.instagram.com/", string domain= ".instagram.com")
        {
            RemoteWebDriver driver = null;
            try
            {
                var cookiePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
                var encKeyPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Local State";


                driver = new ChromeDriver(@"C:\Users\lzx\source\repos\Crawler.Demo\Crawler.API");

                driver.Navigate().GoToUrl(url);

                ReadCookie rc = new ReadCookie();
                SeleCookie sc = rc.GetCookies(cookiePath, encKeyPath);
                var collection = sc.GetType().GetProperties().Where(x => !x.Name.Equals("Item")).Select(x => x.Name).GetEnumerator();
                while (collection.MoveNext())
                {
                    var current = collection.Current;
                    driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(current, sc[current]));
                }
                driver.Url = url;
            }
            catch(Exception ex)
            {
                driver.Quit();
                throw ex;
            }
            finally
            {
                driver.Quit();
            }
            Assert.IsTrue(true);
        }

        public void HttpGetTest()
        {
            var cookiePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
            var encKeyPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Local State";
            ReadCookie rc = new ReadCookie();
            var url = "https://www.instagram.com/graphql/query/?query_hash=c76146de99bb02f6415203be841dd25a&variables=%7B%22id%22%3A%22460563723%22%2C%22include_reel%22%3Atrue%2C%22fetch_mutual%22%3Afalse%2C%22first%22%3A12%2C%22after%22%3A%22QVFCUUE5N084bG9pUUNjakxRT0lCTUVISXJRRmNPT2VkYThSZGh4OW1DcmdFTFZNMlJ0US1oUjdhRXRJN1RoZFhSUEpINUROdXFLZk9MSkhPbXRiRjByRg%3D%3D%22%7D";
            WebUtils wu = new WebUtils();
            SeleCookie cc = rc.GetCookies(cookiePath, encKeyPath);
            string cookieStr = $@"cookie: ig_did={cc.ig_did}; mid={cc.mid}; csrftoken={cc.csrftoken}; ds_user_id={cc.ds_user_id}; sessionid={cc.sessionid}; rur={cc.rur}; urlgen={cc.urlgen}";
            var getResult = wu.DoGet(HttpUtility.UrlDecode(url), null, "text/html; charset=utf-8", cookieStr);

            Assert.IsTrue(true);
        }
    }
}