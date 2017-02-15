using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace FormUI.Tests.SystemTests.Util
{
    public class BrowserApp : IDisposable
    {
        private IWebDriver _browser;

        public BrowserApp(bool runHeadless)
        {
            if (runHeadless)
                _browser = new PhantomJSDriver();
            else
                _browser = new ChromeDriver();
        }

        public void Dispose()
        {
            using (_browser) { }
        }

        public void GoTo(string action)
        {
            Console.WriteLine("Navigating to: " + action);
            _browser.Navigate().GoToUrl(action);
        }
    }
}
