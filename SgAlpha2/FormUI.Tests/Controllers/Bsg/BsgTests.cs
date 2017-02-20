using System;
using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms.Commands;
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
                ExecutorStub.SetupCommand(It.IsAny<StartBestStartGrant>(), "form123");

                var response = client.Get(BsgActions.AboutYou()).Form<AboutYou>(1)
                    .SetText(m => m.FirstName, "first name")
                    .Submit(client);

                ExecutorStub.Executed<StartBestStartGrant>(0).ShouldBeEquivalentTo(new StartBestStartGrant
                {
                    AboutYou = new AboutYou { FirstName = "first name" },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ExpectedChildren("form123"));
            });
        }

        [Test]
        public void AboutYou_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.AboutYou()).Form<AboutYou>(1)
                    .SetDate(m => m.DateOfBirth, "in", "va", "lid")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void ExpectedChildren_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .SetDate(m => m.ExpectancyDate, "01", "02", "2003")
                    .SetText(m => m.ExpectedBabyCount, "2")
                    .Submit(client);

                ExecutorStub.Executed<AddExpectedChildren>(0).ShouldBeEquivalentTo(new AddExpectedChildren
                {
                    FormId = "form123",
                    ExpectedChildren = new ExpectedChildren
                    {
                        ExpectancyDate = new DateTime(2003, 02, 01),
                        ExpectedBabyCount = 2,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Complete());
            });
        }

        [Test]
        public void ExpectedChildren_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ExpectedChildren("form123")).Form<ExpectedChildren>(1)
                    .SetDate(m => m.ExpectancyDate, "in", "va", "lid")
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
