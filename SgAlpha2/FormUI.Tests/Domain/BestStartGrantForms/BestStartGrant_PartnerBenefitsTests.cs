using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_PartnerBenefitsTests : DomainTest
    {
        [Test]
        public void AddPartnerBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PartnerBenefitsShouldBeValid(form, m => { });

            PartnerBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        protected void PartnerBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void PartnerBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }
    }
}
