using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddGuardianBenefitsTests : DomainTest
    {
        [Test]
        public void Execute_StoresBenefitsDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.GuardianBenefits.Should().BeNull("no data stored before executing command");

            var cmd = new AddGuardianBenefits
            {
                FormId = "form123",
                GuardianBenefits = BenefitsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.GuardianBenefits.Should().NotBeNull();
            updatedForm.GuardianBenefits.HasExistingBenefit.Should().Be(cmd.GuardianBenefits.HasExistingBenefit);
        }
    }
}
