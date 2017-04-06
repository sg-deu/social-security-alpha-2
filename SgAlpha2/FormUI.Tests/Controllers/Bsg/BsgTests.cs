using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Controllers.Home;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgTests : WebTest
    {
        [Test]
        public void BeforeYouApply_POST_StartsForm()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(It.IsAny<StartBestStartGrant>(), new NextSection
                {
                    Id = "form123",
                    Section = Sections.Consent,
                });

                var response = client.Get(BsgActions.BeforeYouApply()).Form<object>(1)
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

                response.Text.ToLower().Should().Contain("you don't seem to qualify");
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

        [Test]
        public void BeforeYouApply_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.BeforeYouApply());

                response.Text.ToLower().Should().Contain("before you apply");
            });
        }

        [Test]
        public void UKVerify_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.UKVerify("form123"));

                response.Text.ToLower().Should().Contain("confirm who you are");
            });
        }

        [Test]
        [Ignore]
        public void DeclarationU16_GET()
        {
            WebAppTest(client =>
            {
                var form = new BestStartGrantBuilder("form123")
                    .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid(ad => ad.Under16(System.DateTime.UtcNow)))
                    .Insert();

                var response = client.Get(BsgActions.Declaration(form.Id));
                response.Text.ToLower().Should().Contain("you are under the age of 16");
            });
        }

        [Test]
        [Ignore]
        public void Declaration_GET()
        {
            WebAppTest(client =>
            {
                //var form = new BestStartGrantBuilder("form123")
                //    .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid(ad => ad.Over25(System.DateTime.UtcNow)))
                //    .Insert();

                var response = client.Get(BsgActions.Declaration("form123"));
                response.Text.ToLower().Should().Contain("the information you've given");
            });
        }
    }
}
