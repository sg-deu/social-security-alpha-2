using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Forms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgGuardianDetailsTests : BsgSectionTest
    {
        [Test]
        public void GuardianDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianDetails });
                response.Doc.Form<RelationDetails>(1).GetText(m => m.RelationshipToApplicant).Should().Be(detail.GuardianDetails.RelationshipToApplicant);
            });
        }

        [Test]
        public void GuardianDetails_POST_CanAddGuardianDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Title, "test title")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianDetails>(0).ShouldBeEquivalentTo(new AddGuardianDetails
                {
                    FormId = "form123",
                    GuardianDetails = new RelationDetails
                    {
                        Title = "test title",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianDetails("form123")).Form<RelationDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void GuardianDetails_GET_PopulatesExistingAddressDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianDetails });
                response.Doc.Form<RelationDetails>(1).GetText(m => m.Address.Line1).Should().Be(detail.GuardianDetails.Address.Line1);
            });
        }

        [Test]
        public void GuardianDetails_POST_CanAddGuardianAddressDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Address.Line1, "line 1")
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianDetails>(0).ShouldBeEquivalentTo(new AddGuardianDetails
                {
                    FormId = "form123",
                    GuardianDetails = new RelationDetails
                    {
                        Address = new Address { Line1 = "line 1" },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }
    }
}
