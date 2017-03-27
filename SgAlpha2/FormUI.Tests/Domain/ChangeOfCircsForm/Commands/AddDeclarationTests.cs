using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddDeclarationTests : DomainTest
    {
        [Test]
        public void Execute_StoresDeclaration()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.Declaration.Should().BeNull("no data stored before executing command");

            var cmd = new AddDeclaration
            {
                FormId = "form123",
                Declaration = DeclarationBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Declaration.Should().NotBeNull();
            updatedForm.Declaration.AgreedToLegalStatement.Should().Be(cmd.Declaration.AgreedToLegalStatement);
        }
    }
}
