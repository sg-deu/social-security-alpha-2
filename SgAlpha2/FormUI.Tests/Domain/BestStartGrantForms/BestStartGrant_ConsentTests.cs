using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_ConsentTests : DomainTest
    {
        [Test]
        public void Consent_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ConsentShouldBeValid(form, m => { });

            ConsentShouldBeInvalid(form, m => m.AgreedToConsent = false);
        }

        protected void ConsentShouldBeValid(BestStartGrant form, Action<Consent> mutator)
        {
            ShouldBeValid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }

        protected void ConsentShouldBeInvalid(BestStartGrant form, Action<Consent> mutator)
        {
            ShouldBeInvalid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }
    }
}
