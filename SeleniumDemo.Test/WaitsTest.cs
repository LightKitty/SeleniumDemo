using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumDemo.Test
{
    [TestClass]
    public class WaitsTest
    {
        /// <summary>
        /// 显式等待
        /// </summary>
        [TestMethod]
        public void ExplicitWaitTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.baidu.com/");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement firstResult = wait.Until(e => e.FindElement(By.Id("su")));

                driver.ExecuteScript($"alert('{firstResult.TagName}')");

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 隐式等待
        /// </summary>
        [TestMethod]
        public void ImplicitWaitTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.baidu.com/");

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                IWebElement dynamicElement = driver.FindElement(By.Id("su"));

                driver.ExecuteScript($"alert('{dynamicElement.TagName}')");

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        ///  设置重试间隔和异常忽略
        /// </summary>
        [TestMethod]
        public void  FluentWaitTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.baidu.com/");

                WebDriverWait wait = new WebDriverWait(driver, timeout: TimeSpan.FromSeconds(5))
                {
                    PollingInterval = TimeSpan.FromSeconds(1),
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                var foo = wait.Until(drv => drv.FindElement(By.Id("foo")));

                Thread.Sleep(3000);
            }
        }
    }
}
