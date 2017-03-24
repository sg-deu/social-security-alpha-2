using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_UKVerifyTests : DomainTest
    {
        [Test]
        public void UKVerifyConsent_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            //AgreedToConsent no longer needed...so, what to test?
            //UKVerifyShouldBeValid(form, m => { });
            //UKVerifyShouldBeInvalid(form, m => m.AgreedToConsent = false);
        }

        protected void UKVerifyShouldBeValid(BestStartGrant form, Action<UKVerify> mutator)
        {
            ShouldBeValid(() => form.AddUKVerify(UKVerifyBuilder.NewValid(mutator)));
        }

        protected void UKVerifyShouldBeInvalid(BestStartGrant form, Action<UKVerify> mutator)
        {
            ShouldBeInvalid(() => form.AddUKVerify(UKVerifyBuilder.NewValid(mutator)));
        }
    }
}
