using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddGuardianPartnerDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresGuardianPartnerDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.GuardianPartnerDetails, RelationDetailsBuilder.NewValid(Part.Part1))
                .Insert();

            existingForm.GuardianPartnerDetails.Address.Line1.Should().BeNull("no data stored before executing command");

            var cmd = new AddGuardianPartnerDetails
            {
                FormId = "form123",
                Part = Part.Part2,
                GuardianPartnerDetails = RelationDetailsBuilder.NewValid(Part.Part2),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.GuardianPartnerDetails.Should().NotBeNull();
            updatedForm.GuardianPartnerDetails.Address.Line1.Should().Be(cmd.GuardianPartnerDetails.Address.Line1);
        }
    }
}
