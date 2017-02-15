using System.Net;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.App_Start;
using FormUI.Controllers.Bsg;
using FormUI.Controllers.Home;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Home
{
    [TestFixture]
    public class HomeTests : WebTest
    {
        [Test]
        public void Index_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HomeActions.Index(), r => r.SetExpectedResponse(HttpStatusCode.Redirect));

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Overview());
            });
        }

        [Test]
        public void Password_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HomeActions.Password());

                response.Doc.Document.Body.TextContent.Should().Contain("password protected");
            });
        }

        [Test]
        public void Password_POST_IncorrectPassword()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HomeActions.Password() + "?name=value").Form<PasswordPostModel>(1)
                    .SetText(m => m.Password, "wrong")
                    .Submit(client);

                response.ActionResultOf<RedirectResult>().Url.Should().BeAction(HomeActions.Password() + "?name=value");
            });
        }

        [Test]
        public void Password_POST_CorrectPassword()
        {
            WebAppTest(client =>
            {
                // stub out authentication response
                var responseAuthenticated = false;
                EntryFilter.Authenticate = r => { responseAuthenticated = true; };

                var url = HomeActions.Password() + $"?{HomeController.PasswordReturnUrlName}={HttpUtility.UrlEncode("http://www.google.com")}";

                var response = client.Get(url).Form<PasswordPostModel>(1)
                    .SetText(m => m.Password, HomeController.PasswordValue)
                    .Submit(client);

                response.ActionResultOf<RedirectResult>().Url.Should().Be("http://www.google.com");
                responseAuthenticated.Should().BeTrue("entry cookie should be added");
            });
        }
    }
}
