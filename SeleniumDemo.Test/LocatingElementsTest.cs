using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDemo.Test
{
    [TestClass]
    public class LocatingElementsTest
    {
        [TestMethod]
        public void LocatingOneElementTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.baidu.com/");

                IWebElement element = driver.FindElement(By.Id("su"));

                driver.FindElement(By.CssSelector("#kw")).SendKeys("cheese" + Keys.Enter);

                Thread.Sleep(3000);

                driver.FindElement(By.CssSelector("#kw")).Clear();

                driver.FindElement(By.CssSelector("#kw")).SendKeys("苹果");

                IWebElement element2 = driver.FindElement(By.CssSelector("#su"));

                element2.Click();

                //driver.ExecuteScript($"alert('{element.TagName}')");

                Thread.Sleep(3000);
            }
        }
    }
}
