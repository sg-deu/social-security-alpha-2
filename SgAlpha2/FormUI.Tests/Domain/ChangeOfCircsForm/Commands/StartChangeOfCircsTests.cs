using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class StartChangeOfCircsTests : DomainTest
    {
        [Test]
        public void Execute_CreatesForm()
        {
            var cmd = new StartChangeOfCircs();

            var nextSection = cmd.Execute();

            var createdForm = Repository.Load<ChangeOfCircs>(nextSection.Id);
            createdForm.Should().NotBeNull("form should be in database");
        }
    }
}
