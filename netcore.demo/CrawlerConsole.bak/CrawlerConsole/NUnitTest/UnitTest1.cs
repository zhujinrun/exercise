using Crawler.Service;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Linq.Expressions;

namespace NUnitTest
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
            
        }

        [Test]
        public void TestSelenuim()
        {
            RemoteWebDriver driver = null;
            try
            {
                driver = new ChromeDriver();
                driver.Url = "https://www.baidu.com";
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (driver != null)
                {
                    driver.Quit();
                }

            }
        }

    }


}