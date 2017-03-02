using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class StartBestStartGrantTests : DomainTest
    {
        [Test]
        public void Execute_CreatesForm()
        {
            var cmd = new StartBestStartGrant();

            var nextSection = cmd.Execute();

            var createdForm = Repository.Load<BestStartGrant>(nextSection.Id);
            createdForm.Should().NotBeNull("form should be in database");

            nextSection.Section.Should().Be(Navigation.Order.First());
        }
    }
}
