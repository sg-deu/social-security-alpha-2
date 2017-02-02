using System;
using System.Web;
using FormUI.Tests.Controllers.Util.Hosting;
using FormUI.Tests.Controllers.Util.Http;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Util
{
    [TestFixture]
    public abstract class WebTest
    {
        private static AspNetTestHost _webApp;

        public static void SetUpWebHost()
        {
            try
            {
                _webApp = AspNetTestHost.For(@"..\..\..\FormUI", typeof(TestEnvironmentProxy));
            }
            catch
            {
                TearDownWebHost();
                throw;
            }
        }

        public static void TearDownWebHost()
        {
            using (_webApp) { }
        }

        protected void WebAppTest(Action<SimulatedHttpClient> test)
        {
            _webApp.Test(client =>
            {
                client.Cookies.Add(new HttpCookie("Alpha2Entry", "allow"));
                test(client);
            });
        }

        private class TestEnvironmentProxy : AppDomainProxy
        {
            public TestEnvironmentProxy()
            {
                MvcApplication.Startup = new FakeStartup();
            }
        }
    }
}
