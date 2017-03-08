using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class ApplicantBenefitsTests
    {
        [Test]
        public void CopyTo_CoversAllDetails()
        {
            var src = ApplicantBenefitsBuilder.NewValid(Part.Part2);
            var dest = new ApplicantBenefits();

            src.CopyTo(dest, Part.Part1);

            dest.HasExistingBenefit.Should().Be(src.HasExistingBenefit);

            src.CopyTo(dest, Part.Part2);

            dest.ShouldBeEquivalentTo(src);
        }
    }
}
