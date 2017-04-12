using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_HealthProfessionalTests : DomainTest
    {
        [Test]
        public void HealthProfessional_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            HealthProfessionalShouldBeValid(form, m => { });

            HealthProfessionalShouldBeInvalid(form, m => m.Pin = null);
        }

        protected void HealthProfessionalShouldBeValid(ChangeOfCircs form, Action<HealthProfessional> mutator)
        {
            ShouldBeValid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }

        protected void HealthProfessionalShouldBeInvalid(ChangeOfCircs form, Action<HealthProfessional> mutator)
        {
            ShouldBeInvalid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }
    }
}
