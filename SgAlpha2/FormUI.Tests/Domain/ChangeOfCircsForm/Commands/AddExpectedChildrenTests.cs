using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddExpectedChildrenTests : DomainTest
    {
        [Test]
        public void Execute_StoresExpectedChildren()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.ExpectedChildren.Should().BeNull("no data stored before executing command");

            var cmd = new AddExpectedChildren
            {
                FormId = "form123",
                ExpectedChildren = ExpectedChildrenBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.ExpectedChildren.Should().NotBeNull();
            updatedForm.ExpectedChildren.ExpectancyDate.Should().Be(cmd.ExpectedChildren.ExpectancyDate);
        }
    }
}
