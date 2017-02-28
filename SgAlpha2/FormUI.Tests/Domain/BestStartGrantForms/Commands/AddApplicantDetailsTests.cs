using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddApplicantDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresApplicantDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.ApplicantDetails.Should().BeNull("no data stored before executing command");

            var cmd = new AddApplicantDetails
            {
                FormId = "form123",
                ApplicantDetails = ApplicantDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.ApplicantDetails.Should().NotBeNull();
            updatedForm.ApplicantDetails.FirstName.Should().Be(cmd.ApplicantDetails.FirstName);
        }
    }
}
