using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddOptionsTests : DomainTest
    {
        [Test]
        public void Execute_StoresOptions()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.Options.Should().BeNull("no data stored before executing command");

            var cmd = new AddOptions
            {
                FormId = "form123",
                Options = OptionsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Options.Should().NotBeNull();
            updatedForm.Options.ChangePersonalDetails.Should().Be(cmd.Options.ChangePersonalDetails);
        }
    }
}
