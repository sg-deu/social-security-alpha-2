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
                .With(f => f.GuardianDetails, RelationDetailsBuilder.NewValid())
                .Insert();

            // address now added with main details
            existingForm.GuardianDetails.Address.Line1.Should().NotBeNull();

            var cmd = new AddGuardianDetails
            {
                FormId = "form123",
                GuardianDetails = RelationDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.GuardianDetails.Should().NotBeNull();
            updatedForm.GuardianDetails.Address.Line1.Should().Be(cmd.GuardianDetails.Address.Line1);
        }
    }
}
