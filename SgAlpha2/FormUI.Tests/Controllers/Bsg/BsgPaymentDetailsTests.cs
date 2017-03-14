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
    public class BsgPaymentDetailsTests : BsgSectionTest
    {
        [Test]
        public void PaymentDetails_GET_PopulatesExistingDetails()
        {
            WebAppTest(client =>
            {
                var detail = NewBsgDetail("form123");
                ExecutorStub.SetupQuery(It.IsAny<FindBsgSection>(), detail);

                var response = client.Get(BsgActions.PaymentDetails(detail.Id));

                ExecutorStub.Executed<FindBsgSection>(0).ShouldBeEquivalentTo(new FindBsgSection { FormId = detail.Id, Section = Sections.PaymentDetails });
                response.Doc.Form<PaymentDetails>(1).GetText(m => m.NameOfAccountHolder).Should().Be(detail.PaymentDetails.NameOfAccountHolder);
            });
        }

        [Test]
        public void PaymentDetails_POST_StoresData()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.PaymentDetails("form123")).Form<PaymentDetails>(1)
                    .SelectNo(m => m.LackingBankAccount)
                    .SetText(m => m.NameOfAccountHolder, "test name")
                    .SetText(m => m.NameOfBank, "test bank")
                    .SetText(m => m.SortCode, "01-02-03")
                    .SetText(m => m.AccountNumber, "01234567")
                    .SetText(m => m.RollNumber, "roll/number")
                    .Submit(client);

                ExecutorStub.Executed<AddPaymentDetails>(0).ShouldBeEquivalentTo(new AddPaymentDetails
                {
                    FormId = "form123",
                    PaymentDetails = new PaymentDetails
                    {
                        LackingBankAccount = false,
                        NameOfAccountHolder = "test name",
                        NameOfBank = "test bank",
                        SortCode = "01-02-03",
                        AccountNumber = "01234567",
                        RollNumber = "roll/number",
                    },
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

                var response = client.Get(BsgActions.PaymentDetails("form123")).Form<AddPaymentDetails>(1)
                    .SubmitName("", client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                response.Doc.Find(".validation-summary-errors").Should().NotBeNull();
            });
        }
    }
}
