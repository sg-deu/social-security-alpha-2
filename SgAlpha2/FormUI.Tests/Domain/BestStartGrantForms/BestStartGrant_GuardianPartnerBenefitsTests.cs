using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_GuardianPartnerBenefitsTests : DomainTest
    {
        [Test]
        public void AddGuardianPartnerBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianPartnerBenefitsShouldBeValid(form, m => { });

            GuardianPartnerBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        protected void GuardianPartnerBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddGuardianPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianPartnerBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }
    }
}
