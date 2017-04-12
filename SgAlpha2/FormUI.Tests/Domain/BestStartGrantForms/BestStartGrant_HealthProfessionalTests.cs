using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_HealthProfessionalTests : DomainTest
    {
        [Test]
        public void AddHealthProfessional_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            HealthProfessionalShouldBeValid(form, m => { });

            HealthProfessionalShouldBeInvalid(form, m => m.Invalid());
            HealthProfessionalShouldBeInvalid(form, m => m.Pin = null);
        }

        protected void HealthProfessionalShouldBeValid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            ShouldBeValid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }

        protected void HealthProfessionalShouldBeInvalid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            ShouldBeInvalid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }
    }
}
