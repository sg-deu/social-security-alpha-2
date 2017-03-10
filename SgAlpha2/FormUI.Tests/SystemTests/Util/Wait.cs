using System;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;

namespace FormUI.Tests.SystemTests.Util
{
    public static class Wait
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(20);
        private static          TimeSpan Timeout        = DefaultTimeout;

        public static void For(IWebDriver driver, Action action)
        {
            For(driver, Timeout, action);
        }

        public static void For(IWebDriver driver, TimeSpan timeout, Action action)
        {
            var until = DateTime.Now + timeout;

            WaitUntil(until, action);

            if (driver != null)
                WaitUntil(until, () =>
                {
                    var js = (IJavaScriptExecutor)driver;
                    var active = js.ExecuteScript("return jQuery.active + $(':animated').length;");
                    active.Should().NotBeNull();
                    active.GetType().Should().Be(typeof(long));
                    active.Should().Be(0L);
                });
        }

        private static void WaitUntil(DateTime until, Action action)
        {
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch
                {
                    if (DateTime.Now > until)
                        throw;

                    Thread.Sleep(0);
                }
            }
        }
    }
}
