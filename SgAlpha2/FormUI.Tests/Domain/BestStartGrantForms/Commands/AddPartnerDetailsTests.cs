using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddPartnerDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresPartnerDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.PartnerDetails, RelationDetailsBuilder.NewValid())
                .Insert();

            existingForm.PartnerDetails.Address.Line1.Should().NotBeNull(); //BeNull("no data stored before executing command");

            var cmd = new AddPartnerDetails
            {
                FormId = "form123",
                PartnerDetails = RelationDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.PartnerDetails.Should().NotBeNull();
            updatedForm.PartnerDetails.Address.Line1.Should().Be(cmd.PartnerDetails.Address.Line1);
        }
    }
}
