using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_PaymentDetailsTests : DomainTest
    {
        [Test]
        public void AddPaymentDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => { });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = false; m.RollNumber = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.NameOfAccountHolder = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.NameOfBank = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.SortCode = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.AccountNumber = null; });

            PaymentDetailsShouldBeInvalid(form, m => m.LackingBankAccount = null);
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfAccountHolder = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfBank = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.SortCode = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.AccountNumber = null; });
        }

        [Test]
        public void AddPaymentDetails_SortCodeValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => m.SortCode = "01-02-03");

            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01 02-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-02.03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "x1-02-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-0x-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-02-.3");
        }

        [Test]
        public void AddPaymentDetails_AccountNumberValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => m.AccountNumber = "0");
            PaymentDetailsShouldBeValid(form, m => m.AccountNumber = "01234567");

            PaymentDetailsShouldBeInvalid(form, m => m.AccountNumber = "x");
            PaymentDetailsShouldBeInvalid(form, m => m.AccountNumber = " 1 ");
        }

        protected void PaymentDetailsShouldBeValid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            ShouldBeValid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }

        protected void PaymentDetailsShouldBeInvalid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }
    }
}
