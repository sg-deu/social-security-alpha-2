using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddApplicantDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresApplicantDetails()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.ApplicantDetails.Should().BeNull("no data stored before executing command");

            var cmd = new AddApplicantDetails
            {
                FormId = "form123",
                ApplicantDetails = ApplicantDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.ApplicantDetails.Should().NotBeNull();
            updatedForm.ApplicantDetails.FullName.Should().Be(cmd.ApplicantDetails.FullName);
        }
    }
}
