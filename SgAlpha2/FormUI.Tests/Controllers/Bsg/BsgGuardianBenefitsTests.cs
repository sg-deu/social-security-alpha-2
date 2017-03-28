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
    public class BsgGuardianBenefitsTests : BsgSectionTest
    {
        [Test]
        public void GuardianBenefits_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.GuardianBenefits(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.GuardianBenefits });
                response.Doc.Form<Benefits>(1).GetConfirm(m => m.HasIncomeSupport).Should().Be(detail.GuardianBenefits.HasIncomeSupport);
            });
        }

        [Test]
        public void GuardianBenefits_POST_CanAddGuardianBenefits()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.GuardianBenefits("form123")).Form<Benefits>(1)
                    .SelectConfirm(m => m.HasIncomeSupport, true)
                    .Submit(client);

                ExecutorStub.Executed<AddGuardianBenefits>(0).ShouldBeEquivalentTo(new AddGuardianBenefits
                {
                    FormId = "form123",
                    GuardianBenefits = new Benefits
                    {
                        HasIncomeSupport = true,
                    },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void GuardianBenefits_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddGuardianBenefits, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(BsgActions.GuardianBenefits("form123")).Form<Benefits>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
