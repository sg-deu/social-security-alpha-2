using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class StartBestStartGrantTests : DomainTest
    {
        [Test]
        public void Execute_CreatesForm()
        {
            var aboutYou = AboutYouBuilder.NewValid(m =>
                m.FirstName = "unit test");

            var cmd = new StartBestStartGrant
            {
                AboutYou = aboutYou,
            };

            var id = cmd.Execute();

            var createdForm = Repository.Query<BestStartGrant>().ToList().FirstOrDefault();
            createdForm.Should().NotBeNull("form should be in database");
            createdForm.AboutYou.FirstName.Should().Be("unit test");

            createdForm.Id.Should().Be(id);
        }
    }
}
