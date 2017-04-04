using System.Net;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Controllers.Util.Html;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Coc
{
    [TestFixture]
    public class CocPaymentDetailsTests : CocSectionTest
    {
        [Test]
        public void PaymentDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewCocDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindCocSection>(), detail);

                var response = client.Get(CocActions.PaymentDetails(detail.Id));

                ExecutorStub.Executed<FindCocSection>(0).ShouldBeEquivalentTo(new FindCocSection { FormId = detail.Id, Section = Sections.PaymentDetails });
                response.Doc.Form<PaymentDetails>(1).GetText(m => m.NameOfAccountHolder).Should().Be(detail.PaymentDetails.NameOfAccountHolder);
            });
        }

        [Test]
        public void PaymentDetails_POST_PopulatesPaymentDetails()
        {
            WebAppTest(client =>
            {
                var response = client.Get(CocActions.PaymentDetails("form123")).Form<PaymentDetails>(1)
                    .SetText(m => m.NameOfAccountHolder, "test full name")
                    .Submit(client);

                ExecutorStub.Executed<AddPaymentDetails>(0).ShouldBeEquivalentTo(new AddPaymentDetails
                {
                    FormId = "form123",
                    PaymentDetails = new PaymentDetails { NameOfAccountHolder = "test full name" },
                });

                response.ActionResultOf<RedirectResult>().Url.Should().NotBeNullOrWhiteSpace();
            });
        }

        [Test]
        public void PaymentDetails_POST_ErrorsAreDisplayed()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand<AddPaymentDetails, NextSection>((cmd, def) => { throw new DomainException("simulated logic error"); });

                var response = client.Get(CocActions.PaymentDetails("form123")).Form<PaymentDetails>(1)
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
