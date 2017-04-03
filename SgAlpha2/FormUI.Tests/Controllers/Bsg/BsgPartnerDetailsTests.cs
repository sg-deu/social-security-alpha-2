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
    public class BsgPartnerDetailsTests : BsgSectionTest
    {
        [Test]
        public void PartnerDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.PartnerDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.PartnerDetails });

                var form = response.Doc.Form<RelationDetails>(1);

                form.GetText(m => m.Title).Should().Be(detail.PartnerDetails.Title);
                form.Get(m => m.RelationshipToApplicant).Length.Should().Be(0, "Should not ask partner's relationship");

                form.GetText(m => m.Address.Line1).Should().Be(detail.PartnerDetails.Address.Line1);
                form.Get(m => m.InheritAddress).Length.Should().Be(1, "option to inherit address should be visible");

                form.WhenCheckedShows(m => m.InheritAddress, "inherited-address");
                form.WhenUncheckedShows(m => m.InheritAddress, "new-address");
            });
        }

        [Test]
        public void PartnerDetails_POST_CanAddPartnerDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.PartnerDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Title, "test title")
                    .Submit(client);

                ExecutorStub.Executed<AddPartnerDetails>(0).ShouldBeEquivalentTo(new AddPartnerDetails
                {
                    FormId = "form123",
                    PartnerDetails = new RelationDetails
                    {
                        Title = "test title",
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void PartnerDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddPartnerDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.PartnerDetails("form123")).Form<RelationDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }

        [Test]
        public void PartnerDetails_POST_CanAddPartnerAddressDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.PartnerDetails("form123")).Form<RelationDetails>(1)
                    .SetText(m => m.Address.Line1, "line 1")
                    .Submit(client);

                ExecutorStub.Executed<AddPartnerDetails>(0).ShouldBeEquivalentTo(new AddPartnerDetails
                {
                    FormId = "form123",
                    PartnerDetails = new RelationDetails
                    {
                        Address = new Address { Line1 = "line 1" },
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }
    }
}
