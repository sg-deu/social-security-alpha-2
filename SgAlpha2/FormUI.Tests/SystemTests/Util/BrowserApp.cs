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
            {
                var service = PhantomJSDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                _browser = new PhantomJSDriver(service);
            }
            else
            {
                _browser = new ChromeDriver();
            }
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
