using System;
using System.Drawing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumDemo.Test
{
    [TestClass]
    public class BrowserManipulationTest
    {
        /// <summary>
        /// 打开新窗口并切换
        /// </summary>
        [TestMethod]
        public void SwitchingWindowsOrTabsTest()
        {
            var options = new ChromeOptions();
            options.AddArgument("--user-data-dir=C:/Users/Yang/AppData/Local/Google/Chrome/User Data"); //置顶用户文件夹路径
            options.AddArgument("--profile-directory=Default"); //指定用户
            //m_Options.AddArgument("--disable-extensions"); //禁用插件
            var driver = new ChromeDriver(@"D:\WebDriver\bin", options); //声明chrome驱动器

            driver.Navigate().GoToUrl(@"https://www.baidu.com/"); //跳转网址

            //Store the ID of the original window
            string originalWindow = driver.CurrentWindowHandle;

            //Check we don't have other windows open already
            Assert.AreEqual(driver.WindowHandles.Count, 1);

            //Click the link which opens in a new window
            driver.FindElement(By.LinkText("地图")).Click();

            //声明等待器（最多等待10s）
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); 

            //Wait for the new window or tab
            wait.Until(wd => wd.WindowHandles.Count == 2);

            //Loop through until we find a new window handle
            foreach (string window in driver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    driver.SwitchTo().Window(window);
                    break;
                }
            }
            //Wait for the new tab to finish loading content
            wait.Until(wd => wd.Title == "百度地图");

            System.Console.WriteLine("driver.Title: " + driver.Title);

            //Close the tab or window
            driver.Close();

            //Switch back to the old tab or window
            driver.SwitchTo().Window(originalWindow);

            System.Console.WriteLine("driver.Title: " + driver.Title);

            //Console.ReadLine();

            Thread.Sleep(5000);

            driver.Quit();
        }

        //打开新窗口并跳转
        [TestMethod]
        public void CreateNewWindowOrNewTabAndSwitch()
        {
            var driver = new ChromeDriver(@"D:\WebDriver\bin"); //声明chrome驱动器

            //// JavaScript Method
            //driver.ExecuteScript("window.open('http://www.baidu.com','_blank');");

            //Note: This feature works with Selenium 4 and later versions.

            // Opens a new tab and switches to new tab
            driver.SwitchTo().NewWindow(WindowType.Tab);

            // Opens a new window and switches to new window
            driver.SwitchTo().NewWindow(WindowType.Window);

            Console.ReadLine();
        }

        //获取和调整浏览器大小
        [TestMethod]
        public void WindowSizeTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                //Access each dimension individually
                int width = driver.Manage().Window.Size.Width;
                int height = driver.Manage().Window.Size.Height;

                //Or store the dimensions and query them later
                System.Drawing.Size size = driver.Manage().Window.Size;
                int width1 = size.Width;
                int height1 = size.Height;

                driver.Manage().Window.Size = new Size(1600, 900);

                //最大化
                driver.Manage().Window.Maximize();

                //最小化
                driver.Manage().Window.Minimize();

                //全屏
                driver.Manage().Window.FullScreen();
            }
        }

        /// <summary>
        /// 获取&调整浏览器位置
        /// </summary>
        [TestMethod]
        public void WindowPositionTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                //Access each dimension individually
                int x = driver.Manage().Window.Position.X;
                int y = driver.Manage().Window.Position.Y;

                //Or store the dimensions and query them later
                Point position = driver.Manage().Window.Position;
                int x1 = position.X;
                int y1 = position.Y;

                // Move the window to the top left of the primary monitor
                driver.Manage().Window.Position = new Point(0, 0);
            }
        }

        //截图保存
        [TestMethod]
        public void TakeScreenshotTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.selenium.dev/documentation/en/webdriver/browser_manipulation/#takescreenshot");
                Screenshot screenshot = (driver as ITakesScreenshot).GetScreenshot();
                screenshot.SaveAsFile("screenshot.png", ScreenshotImageFormat.Png); // Format values are Bmp, Gif, Jpeg, Png, Tiff

            }
        }

        //元素截图保存
       [TestMethod]
       public void TakeElementScreenshotTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://www.selenium.dev/documentation/en/webdriver/browser_manipulation/#takescreenshot");

                // Fetch element using FindElement
                var webElement = driver.FindElement(By.CssSelector("h1"));

                // Screenshot for the element
                var elementScreenshot = (webElement as ITakesScreenshot).GetScreenshot();
                elementScreenshot.SaveAsFile("screenshot_of_element.png");
            }
        }

        //执行JavaScript脚本
        [TestMethod]
        public void ExecuteScriptTest()
        {
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin")) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://cn.bing.com/");

                driver.ExecuteScript("window.open('http://www.baidu.com','_blank');");

                string text = (string)driver.ExecuteScript("return document.title");

                Thread.Sleep(3000);
            }
        }
    }
}
