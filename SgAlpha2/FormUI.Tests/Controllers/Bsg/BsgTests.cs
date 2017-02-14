using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgTests : WebTest
    {
        [Test]
        public void Overview_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Overview());

                response.Doc.Document.Body.TextContent.Should().Contain("Overview");
            });
        }

        [Test]
        public void AboutYou_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.AboutYou());

                response.Doc.Document.Body.TextContent.Should().Contain("About You");
            });
        }

        [Test]
        public void AboutYou_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                AboutYou dto = null;
                BsgFacade.Start = postedModel => dto = postedModel;

                var response = client.Get(BsgActions.AboutYou()).Form<AboutYou>(1)
                    .SetText(m => m.FirstName, "first name")
                    .SetDate(m => m.DateOfBirth, "01", "02", "2003")
                    .SetDate(m => m.CurrentAddress.DateMovedIn , "02", "03", "2004")
                    .Submit(client);

                dto.Should().NotBeNull("controller should call BsgStart()");
                dto.FirstName.Should().Be("first name");

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Complete());
            });
        }

        [Test]
        public void AboutYou_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.AboutYou()).Form<AboutYou>(1)
                    .SetDate(m => m.DateOfBirth, "in", "va", "lid")
                    .SetText(m => m.FirstName, "first name")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void Complete_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Complete());

                response.Text.ToLower().Should().Contain("received");
            });
        }
    }
}
