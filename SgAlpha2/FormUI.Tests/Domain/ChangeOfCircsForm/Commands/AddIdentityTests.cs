using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddIdentityTests : DomainTest
    {
        [Test]
        public void Execute_StoresConsent()
        {
            new BestStartGrantBuilder("form").PreviousApplicationFor("existing.identity@existing.com").Insert();

            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.UserId.Should().BeNull("no data stored before executing command");

            var cmd = new AddIdentity
            {
                FormId = "form123",
                Identity = "existing.identity@existing.com",
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.UserId.Should().NotBeNull();
            updatedForm.UserId.Should().Be(cmd.Identity);
        }
    }
}
