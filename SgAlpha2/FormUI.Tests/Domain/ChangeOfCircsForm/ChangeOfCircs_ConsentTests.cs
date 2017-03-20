using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_ConsentTests : DomainTest
    {
        [Test]
        public void Consent_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            ConsentShouldBeValid(form, m => { });

            ConsentShouldBeInvalid(form, m => m.AgreedToConsent = false);
        }

        protected void ConsentShouldBeValid(ChangeOfCircs form, Action<Consent> mutator)
        {
            ShouldBeValid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }

        protected void ConsentShouldBeInvalid(ChangeOfCircs form, Action<Consent> mutator)
        {
            ShouldBeInvalid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }
    }
}
