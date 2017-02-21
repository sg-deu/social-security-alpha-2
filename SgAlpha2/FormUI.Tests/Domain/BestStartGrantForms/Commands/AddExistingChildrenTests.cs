using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddExistingChildrenTests : DomainTest
    {
        [Test]
        public void Execute_StoresExistingDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.AboutYou, AboutYouBuilder.NewValid())
                .With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid())
                .Insert();

            existingForm.ExistingChildren.Should().BeNull("no data stored before executing command");

            var cmd = new AddExistingChildren
            {
                FormId = "form123",
                ExistingChildren = ExistingChildrenBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.ExistingChildren.Should().NotBeNull();
            updatedForm.ExistingChildren.Children.Count.Should().Be(cmd.ExistingChildren.Children.Count);
        }
    }
}
