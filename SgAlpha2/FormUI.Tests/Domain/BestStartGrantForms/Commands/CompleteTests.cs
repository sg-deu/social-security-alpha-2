using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class CompleteTests : DomainTest
    {
        [Test]
        public void Execute_StoresDeclaration()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.Declaration.Should().BeNull("no data stored before executing command");

            var cmd = new Complete
            {
                FormId = "form123",
                Declaration = DeclarationBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.Declaration.Should().NotBeNull();
            updatedForm.Declaration.AgreedToLegalStatement.Should().BeTrue();
        }
    }
}
