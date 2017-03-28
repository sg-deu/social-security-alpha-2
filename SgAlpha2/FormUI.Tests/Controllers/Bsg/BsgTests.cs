using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
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
        public void Overview_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<StartBestStartGrant>(), new NextSection
                {
                    Id = "form123",
                    Section = Sections.Consent,
                });

                var response = client.Get(BsgActions.Overview()).Form<object>(1)
                    .Submit(client);

                ExecutorStub.Executed<StartBestStartGrant>().Length.Should().Be(1);

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void FirstSectionDoesNotNeedId()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupQuery<FindBsgSection, BsgDetail>((cmd, r) => { throw new DomainException("cannot call FindBsgSection with a null id"); });

                var firstSection = Navigation.Order.First();
                var firstAction = SectionActionStrategy.For(firstSection).Action(null);

                client.Get(firstAction);
            });
        }

        [Test]
        public void NextSection_RedirectToComplete()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<AddApplicantDetails>(), new NextSection { Type = NextType.Complete });

                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1).Submit(client);

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Complete());
            });
        }

        [Test]
        public void NextSection_RedirectToIneligible()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<AddApplicantDetails>(), new NextSection { Type = NextType.Ineligible, Id = "form123" });

                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1).Submit(client);

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.Ineligible("form123"));
            });
        }

        [Test]
        public void NextSection_RedirectToNextSection()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<AddApplicantDetails>(), new NextSection { Type = NextType.Section, Id = "form123", Section = Sections.ExistingChildren });

                var response = client.Get(BsgActions.ApplicantDetails("form123")).Form<ApplicantDetails>(1).Submit(client);

                response.ActionResultOf<RedirectResult>().Url.Should().Be(BsgActions.ExistingChildren("form123"));
            });
        }

        [Test]
        public void Ineligible_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Ineligible("form"));

                response.Text.ToLower().Should().Contain("based on your answers");
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
