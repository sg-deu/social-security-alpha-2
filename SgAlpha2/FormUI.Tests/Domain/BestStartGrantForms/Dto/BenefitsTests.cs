using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class BenefitsTests : AbstractTest
    {
        [Test]
        public void HasExistingBenefit_ReturnsNullWhenNothingIndicated()
        {
            BenefitsBuilder.NewEmpty().HasExistingBenefit().Should().BeNull();
        }

        [Test]
        public void HasExistingBenefit_ReturnsYes_WhenBenefitSelected()
        {
            BenefitsBuilder.NewEmpty(b => b.HasIncomeSupport = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasIncomeBasedJobseekersAllowance = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasIncomeRelatedEmplymentAndSupportAllowance = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasUniversalCredit = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasChildTaxCredit = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasWorkingTextCredit = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasHousingBenefit = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
            BenefitsBuilder.NewEmpty(b => b.HasPensionCredit = true).HasExistingBenefit().Should().Be(YesNoDk.Yes);
        }

        [Test]
        public void HasExistingBenefit_ReturnsDk_WhenDontKnown()
        {
            BenefitsBuilder.NewEmpty(b => b.Unknown = true).HasExistingBenefit().Should().Be(YesNoDk.DontKnow);
        }

        [Test]
        public void HasExistingBenefit_ReturnsNo_WhenNoSelected()
        {
            BenefitsBuilder.NewEmpty(b => b.None = true).HasExistingBenefit().Should().Be(YesNoDk.No);
        }

        [Test]
        public void HasExistingBenefit_DoesNotReturnNull_WhenSomethingIsSet()
        {
            var props = typeof(Benefits).GetProperties();
            foreach (var prop in props)
            {
                var benefit = new Benefits();
                prop.SetValue(benefit, true);
                benefit.HasExistingBenefit().Should().NotBeNull("should not return null when {0} is true", prop.Name);
            }
        }
    }
}
