using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddHealthProfessionalTests : DomainTest
    {
        [Test]
        public void Execute_StoresHealthProfessional()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.HealthProfessional.Should().BeNull("no data stored before executing command");

            var cmd = new AddHealthProfessional
            {
                FormId = "form123",
                HealthProfessional = HealthProfessionalBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.HealthProfessional.Should().NotBeNull();
            updatedForm.HealthProfessional.Pin.Should().Be(cmd.HealthProfessional.Pin);
        }
    }
}
