using System;
using System.Linq;
using FluentAssertions;
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
            try
            {
                if (runHeadless)
                {
                    var service = PhantomJSDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    _browser = new PhantomJSDriver(service);
                }
                else
                {
                    var service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    _browser = new ChromeDriver(service);
                }

                _browser.Navigate().GoToUrl(DevWebServer.RootUrl);

                // PhantomJS couldn't set cookies here, so use JS instead
                // https://github.com/detro/ghostdriver/issues/477#issuecomment-240915786
                var scriptExecutor = (IJavaScriptExecutor)_browser;
                scriptExecutor.ExecuteScript("document.cookie = 'cookie-notification-acknowledged=yes; path=/;'");
                scriptExecutor.ExecuteScript("document.cookie = 'Alpha2Entry=allow; path=/;'");
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            using (_browser) { }
        }

        public void GoTo(string action)
        {
            Console.WriteLine("Navigating to '{0}' ", action);
            Wait.For(() =>
            {
                action = action.Replace("~/", DevWebServer.RootUrl);
                _browser.Navigate().GoToUrl(action);
            });
        }

        public void VerifyCanSeeText(string text)
        {
            Console.WriteLine("Verify can see text '{0}'", text);
            Wait.For(() =>
            {
                var body = _browser.FindElement(By.TagName("body"));
                body.Text.Should().Contain(text);
            });
        }

        public void ClickLinkButton(string linkText)
        {
            Console.WriteLine("Click link button '{0}'", linkText);
            Wait.For(() =>
            {
                var links = _browser.FindElements(By.CssSelector("a.button"));
                var link = links.Where(l => string.Equals(l.Text, linkText, StringComparison.OrdinalIgnoreCase)).Single();
                link.Click();
            });
        }

        public FormModel<T> FormFormModel<T>()
        {
            return new FormModel<T>(this);
        }

        public void TypeText(string inputName, string text)
        {
            Console.WriteLine("Type text '{0}' into '{1}'", text, inputName);
            Wait.For(() =>
            {
                var input = _browser.FindElement(By.CssSelector($"input[name={inputName}]"));
                input.SendKeys(text);
            });
        }
    }
}
