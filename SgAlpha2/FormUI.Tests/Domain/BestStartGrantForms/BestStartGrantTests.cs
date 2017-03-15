using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrantTests : DomainTest
    {
        [Test]
        public void NextSectionClearsSkippedSections()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid(ad => ad.Under16(TestNowUtc.Value)))
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewValid())
                .With(f => f.Declaration, DeclarationBuilder.NewValid())
                .Insert();

            form.AddExistingChildren(ExistingChildrenBuilder.NewValid());

            form = Repository.Load<BestStartGrant>(form.Id);

            form.Declaration.Should().NotBeNull("should not be overwritten by moving to the next section");
            form.ApplicantBenefits.Should().BeNull("intermediate 'ApplicantBenefits' section should be cleared based on answers");
        }
    }
}
