using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Forms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_PaymentDetailsTests : DomainTest
    {
        [Test]
        public void PaymentDetails_Validation()
        {
            var form = new ChangeOfCircsBuilder("form")
                .With(f => f.ExistingPaymentDetails, PaymentDetailsBuilder.NewValid())
                .Insert();

            PaymentDetailsShouldBeValid(form, m => { });

            PaymentDetailsShouldBeInvalid(form, m => m.AccountNumber = null); // only need to check existing BankDetails validation called
        }

        [Test]
        public void PaymentDetails_WhenNoExistingBankAccount_MustAddBankAccount()
        {
            var form = new ChangeOfCircsBuilder("form")
                .WithCompletedSections(markAsCompleted: false)
                .Insert(coc => coc.ExistingPaymentDetails.WithoutAccount());

            ShouldBeInvalid(() =>
                form.AddPaymentDetails(new PaymentDetails { HasBankAccount = false }));
        }

        protected void PaymentDetailsShouldBeValid(ChangeOfCircs form, Action<PaymentDetails> mutator)
        {
            ShouldBeValid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }

        protected void PaymentDetailsShouldBeInvalid(ChangeOfCircs form, Action<PaymentDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }
    }
}
