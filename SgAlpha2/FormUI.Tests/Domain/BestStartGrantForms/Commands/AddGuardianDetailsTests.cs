using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddGuardianDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresGuardianDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.GuardianDetails, GuardianDetailsBuilder.NewValid(Part.Part1))
                .Insert();

            existingForm.GuardianDetails.Address.Line1.Should().BeNull("no data stored before executing command");

            var cmd = new AddGuardianDetails
            {
                FormId = "form123",
                Part = Part.Part2,
                GuardianDetails = GuardianDetailsBuilder.NewValid(Part.Part2),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.GuardianDetails.Should().NotBeNull();
            updatedForm.GuardianDetails.Address.Line1.Should().Be(cmd.GuardianDetails.Address.Line1);
        }
    }
}
