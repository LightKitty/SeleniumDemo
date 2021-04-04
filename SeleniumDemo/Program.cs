using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumDemo.ConsoleTest
{
    class Program
    {
        //通过窗口名字查找
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lp1, string lp2);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        private static extern void SetForegroundWindow(IntPtr hwnd);

        //获取到该窗口句柄后,可以对该窗口进行操作
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        // 取得一个窗体的标题（caption）文字，或者一个控件的内容（在vb里使用：使用vb窗体或控件的caption或text属性）
        [DllImport("user32", EntryPoint = "GetWindowText", SetLastError = false,
        CharSet = CharSet.Auto, ExactSpelling = false,
        CallingConvention = CallingConvention.StdCall)]
        public static extern int FindWindow(
            int lpClassName,
            string lpWindowName
        );

        // 在窗口列表中寻找与指定条件相符的第一个子窗口
        [DllImport("user32", EntryPoint = "FindWindowEx", SetLastError = false,
        CharSet = CharSet.Auto, ExactSpelling = false,
        CallingConvention = CallingConvention.StdCall)]

        public static extern int FindWindowEx2(
            IntPtr hwndParent,
            int hWnd2,
            string lpsz1,
            string lpsz2
        );

        public delegate bool CallBack(IntPtr hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        static void Main(string[] args)
        {

            var options = new ChromeOptions();
            options.AddArgument("--user-data-dir=C:/Users/Yang/AppData/Local/Google/Chrome/User Data"); //置顶用户文件夹路径
            options.AddArgument("--profile-directory=Default"); //指定用户
            //m_Options.AddArgument("--disable-extensions"); //禁用插件
            using (var driver = new ChromeDriver(@"D:\WebDriver\bin", options)) //声明chrome驱动器
            {
                driver.Navigate().GoToUrl("https://member.bilibili.com/platform/upload/video/frame"); //跳转网址

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(wb => wb.FindElements(By.TagName("iframe")).Count() > 0);
                driver.SwitchTo().Frame(1);
                IWebElement firstResult = wait.Until(wb => wb.FindElement(By.Id("bili-upload-btn"))); //查找上传按钮
                Thread.Sleep(100); //bili需要等待资源加载
                firstResult.Click(); //点击上传按钮

                Thread.Sleep(1000);

                IntPtr hWnd = FindWindow(null, "打开");
                //var result = GetAllChildrenWindowHandles(hWnd, 100);
                if (hWnd != null && hWnd != new IntPtr(0))
                {
                    uint WM_SETTEXT = 0xC;
                    IntPtr textHwnd = FindWindowEx(hWnd, IntPtr.Zero, null, "文件名(&N):"); //获取文件名lable句柄
                    IntPtr editor = FindWindowEx(hWnd, textHwnd, null, null); //获取文本框句柄（位于文件名lable后）
                    SendMessage(editor, WM_SETTEXT, IntPtr.Zero, @"E:\地球频道\2.videos\20210402毅力号自拍\导出.mp4");
                    Thread.Sleep(100);
                    
                    IntPtr childHwnd = FindWindowEx(hWnd, IntPtr.Zero, null, "打开(&O)"); //获取按钮的句柄
                    if (childHwnd != IntPtr.Zero)
                    {
                        SendMessage(childHwnd, 0xF5, 0, 0);　//鼠标点击的消息，对于各种消息的数值，大家还是得去API手册
                    }

                    IWebElement imgElement = wait.Until(wb => wb.FindElement(By.CssSelector("#app > div.upload-v2-container > div.upload-v2-step2-container > div.file-content-v2-container > div.normal-v2-container > div.cover-v2-container > div.cover-v2-detail-wrp > div.cover-v2-preview > input[type=file]")));
                    imgElement.SendKeys(@"E:\地球频道\2.videos\20200809\vlcsnap-2020-08-09-23h21m21s931.png");

                    wait.Until(wb => wb.FindElement(By.CssSelector("#app > div.common-modal-container > div > div.common-modal-foot > div > div > div:nth-child(1)"))).Click();

                    IWebElement titleElement = driver.FindElement(By.CssSelector("#app > div.upload-v2-container > div.upload-v2-step2-container > div.file-content-v2-container > div.normal-v2-container > div.content-title-v2-container > div.content-title-v2-input-wrp > div > div > input"));
                    titleElement.Clear();
                    titleElement.SendKeys("我是标题");

                    //分类
                    //wait.Until(wb => wb.FindElement(By.CssSelector("#type-list-v2-container > div.type-list-v2-selector-wrp > div > div"))).Click();

                    //标签
                    IWebElement tagElement = driver.FindElement(By.CssSelector("#content-tag-v2-container > div.content-tag-v2-input-wrp > div > div.input-box-v2-1-instance > input"));
                    tagElement.SendKeys($"天文{Keys.Enter}");
                    Thread.Sleep(200);
                    tagElement.SendKeys($"宇宙{Keys.Enter}");
                    Thread.Sleep(200);
                    tagElement.SendKeys($"地球{Keys.Enter}");

                    //简介
                    driver.FindElement(By.CssSelector("#app > div.upload-v2-container > div.upload-v2-step2-container > div.file-content-v2-container > div.normal-v2-container > div.content-desc-v2-container > div.content-desc-v2-text-wrp > div > textarea")).SendKeys("哈哈简介！");
                }

                Console.ReadLine();
            }


            //System.Console.WriteLine("driver.Url: " + driver.Url);
            //driver.Navigate().Back(); //后退
            //driver.Navigate().Forward(); //前进
            //driver.Navigate().Refresh(); //刷新
            //System.Console.WriteLine("driver.Title: " + driver.Title);
            //System.Console.WriteLine("river.CurrentWindowHandle: " + driver.CurrentWindowHandle); //获取窗口句柄


            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //driver.FindElement(By.Name("q")).SendKeys("cheese" + Keys.Enter);
            //wait.Until(webDriver => webDriver.FindElement(By.CssSelector("h3>div")).Displayed);
            //IWebElement firstResult = driver.FindElement(By.CssSelector("h3>div"));
            //Console.WriteLine(firstResult.GetAttribute("textContent"));
        }

        /// <summary>
        /// 查找所有子窗口
        /// </summary>
        /// <param name="hParent"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        static ArrayList GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
        {
            ArrayList result = new ArrayList();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    break;
                }
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }

        ///// <summary>
        ///// 查找窗体上控件句柄
        ///// </summary>
        ///// <param name="hwnd">父窗体句柄</param>
        ///// <param name="lpszWindow">控件标题(Text)</param>
        ///// <param name="bChild">设定是否在子窗体中查找</param>
        ///// <returns>控件句柄，没找到返回IntPtr.Zero</returns>
        //private static IntPtr FindWindowEx(IntPtr hwnd, string lpszWindow, bool bChild)
        //{
        //    IntPtr iResult = IntPtr.Zero;
        //    // 首先在父窗体上查找控件
        //    iResult = FindWindowEx(hwnd, 0, null, lpszWindow);
        //    // 如果找到直接返回控件句柄
        //    if (iResult != IntPtr.Zero) return iResult;

        //    // 如果设定了不在子窗体中查找
        //    if (!bChild) return iResult;

        //    // 枚举子窗体，查找控件句柄
        //    int i = EnumChildWindows(
        //    hwnd,
        //    (h, l) =>
        //    {
        //        IntPtr f1 = FindWindowEx(h, 0, null, lpszWindow);
        //        if (f1 == IntPtr.Zero)
        //            return true;
        //        else
        //        {
        //            iResult = f1;
        //            return false;
        //        }
        //    },
        //    0);
        //    // 返回查找结果
        //    return iResult;

        //}
    }
}
