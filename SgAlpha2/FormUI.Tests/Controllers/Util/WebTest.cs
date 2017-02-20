using System;
using System.Net;
using System.Web;
using FluentAssertions;
using FormUI.Tests.Controllers.Util.Hosting;
using FormUI.Tests.Controllers.Util.Http;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Util
{
    [TestFixture]
    public abstract class WebTest
    {
        private static Lazy<AspNetTestHost> _webApp;

        public static void SetUpWebHost()
        {
            _webApp = new Lazy<AspNetTestHost>(() => AspNetTestHost.For(@"..\..\..\FormUI", typeof(TestEnvironmentProxy)));
        }

        public static void TearDownWebHost()
        {
            if (_webApp.IsValueCreated)
                using (_webApp.Value) { }
        }

        public static ExecutorStub ExecutorStub;

        protected void WebAppTest(Action<SimulatedHttpClient> test)
        {
            _webApp.Value.Test(client =>
            {
                ExecutorStub = new ExecutorStub();

                client.Cookies.Add(new HttpCookie("Alpha2Entry", "allow"));
                test(client);
            });
        }

        protected void VerifyView(string actionUrl)
        {
            WebAppTest(client =>
            {
                var response = client.Get(actionUrl);
                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
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
