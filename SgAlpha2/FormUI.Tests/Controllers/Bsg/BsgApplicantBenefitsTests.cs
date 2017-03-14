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
    public class BsgApplicantBenefitsTests : BsgSectionTest
    {
        [Test]
        public void ApplicantBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.ApplicantBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.ApplicantBenefits });
                response.Doc.Form<Benefits>(1).GetText(m => m.HasExistingBenefit).Should().Be(detail.ApplicantBenefits.HasExistingBenefit.ToString());
            });
        }

        [Test]
        public void ApplicantBenefits_POST_CanAddApplicantBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.ApplicantBenefits("form123")).Form<Benefits>(1)
                    .SetText(m => m.HasExistingBenefit, YesNoDk.No.ToString())
                    .Submit(client);

                ExecutorStub.Executed<AddApplicantBenefits>(0).ShouldBeEquivalentTo(new AddApplicantBenefits
                {
                    FormId = "form123",
                    ApplicantBenefits = new Benefits
                    {
                        HasExistingBenefit = YesNoDk.No,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void ApplicantBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddApplicantBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.ApplicantBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
