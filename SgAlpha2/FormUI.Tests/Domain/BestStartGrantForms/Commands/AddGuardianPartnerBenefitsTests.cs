using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddGuardianPartnerBenefitsTests : DomainTest
    {
        [Test]
        public void Execute_StoresBenefitsDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.GuardianPartnerBenefits.Should().BeNull("no data stored before executing command");

            var cmd = new AddGuardianPartnerBenefits
            {
                FormId = "form123",
                GuardianPartnerBenefits = BenefitsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.GuardianPartnerBenefits.Should().NotBeNull();
            updatedForm.GuardianPartnerBenefits.HasExistingBenefit.Should().Be(cmd.GuardianPartnerBenefits.HasExistingBenefit);
        }
    }
}
