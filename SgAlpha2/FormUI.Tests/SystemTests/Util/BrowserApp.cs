using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace FormUI.Tests.SystemTests.Util
{
    public class BrowserApp : IDisposable
    {
        private static BrowserApp _instance;

        public static void Start(bool runHeadless)
        {
            _instance = new BrowserApp(runHeadless);
        }

        public static void Stop()
        {
            using (_instance) { }
            _instance = null;
        }

        public static BrowserApp Instance()
        {
            return _instance;
        }

        private IWebDriver _browser;

        private BrowserApp(bool runHeadless)
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

        public void FindElement(string testText, By by, Action<IWebElement> verify)
        {
            Console.WriteLine(testText);
            Wait.For(() =>
            {
                var element = _browser.FindElement(by);
                verify(element);
            });
        }

        public void FindElements(string testText, By by, Action<ReadOnlyCollection<IWebElement>> verify)
        {
            Console.WriteLine(testText);
            Wait.For(() =>
            {
                var elements = _browser.FindElements(by);
                verify(elements);
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

        public FormModel<T> FormForModel<T>()
        {
            return new FormModel<T>(this);
        }

        public void TypeText(string name, string text)
        {
            Console.WriteLine("Type text '{0}' into '{1}'", text, name);
            Wait.For(() =>
            {
                var input = _browser.FindElement(By.CssSelector($"input[name='{name}']"));
                input.Clear();
                input.SendKeys(text);
            });
        }

        public void Blur(string name)
        {
            Console.WriteLine("Tab off " + name);
            Wait.For(() =>
            {
                var input = _browser.FindElement(By.CssSelector($"input[name='{name}']"));
                input.SendKeys(Keys.Tab);
            });
        }

        public void GetText(string testText, string name, Action<string> verify)
        {
            Console.WriteLine(testText);
            Wait.For(() =>
            {
                var input = _browser.FindElement(By.CssSelector($"input[name='{name}']"));
                input.Displayed.Should().BeTrue(testText);
                var value = input.GetAttribute("value");
                verify(value);
            });
        }

        public void SelectRadio(string name, string value)
        {
            Console.WriteLine("Select '{0}' for '{1}'", value, name);
            Wait.For(() =>
            {
                var radios = _browser.FindElements(By.CssSelector($"input[name='{name}']"));
                var radio = radios.Where(r => r.GetAttribute("value") == value).Single();
                radio.Click();
            });
        }

        public void Check(string name, bool check)
        {
            var display = check ? "Check" : "Uncheck";
            Console.Write("{0} {1}; ", display, name);
            var after = "";
            Wait.For(() =>
            {
                var checkBox = _browser.FindElement(By.Name(name));

                if (checkBox.Selected && check)
                {
                    after = "already checked";
                }
                else if (checkBox.Selected && !check)
                {
                    checkBox.Click();
                    after = "unchecked";
                }
                else if (!checkBox.Selected && check)
                {
                    checkBox.Click();
                    after = "checked";
                }
                else if (!checkBox.Selected && !check)
                {
                    after = "already unchecked";
                }
            });
            Console.WriteLine(after);
        }

        public void ClickButton(string name)
        {
            Console.WriteLine("Click button ", name);
            Wait.For(() =>
            {
                IList<IWebElement> buttons = _browser.FindElements(By.CssSelector("form[method='post'] button"));

                if (name != null)
                    buttons = buttons.Where(b => b.GetAttribute("name") == name).ToList();

                if (buttons.Count > 1)
                    Assert.Fail("No single button; found: ", string.Join("\n", buttons.Select(b => b.GetAttribute("outerHTML"))));

                if (buttons.Count < 1)
                    Assert.Fail("No buttons found with name = ", name);

                buttons.First().Click();
            });
        }

        public void Submit()
        {
            ClickButton(null);
        }
    }
}
