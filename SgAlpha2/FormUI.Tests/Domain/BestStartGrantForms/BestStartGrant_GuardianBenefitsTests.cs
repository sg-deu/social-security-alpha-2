using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_GuardianBenefitsTests : DomainTest
    {
        [Test]
        public void AddGuardianBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianBenefitsShouldBeValid(form, m => { });

            GuardianBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        protected void GuardianBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddGuardianBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianBenefits(BenefitsBuilder.NewValid(mutator)));
        }
    }
}
