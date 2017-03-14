using System.Net;
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
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgGuardianPartnerBenefitsTests : BsgSectionTest
    {
        [Test]
        public void GuardianPartnerBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianPartnerBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianPartnerBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.GuardianPartnerBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void GuardianPartnerBenefits_POST_CanAddGuardianPartnerBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianPartnerBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianPartnerBenefits>(0).ShouldBeEquivalentTo(new AddGuardianPartnerBenefits
                {
                    FormId = "form123",
                    GuardianPartnerBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianPartnerBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianPartnerBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianPartnerBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
