using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class StartBestStartGrantTests : DomainTest
    {
        [Test]
        public void Execute_CreatesForm()
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(m =>
                m.FirstName = "unit test");

            var cmd = new StartBestStartGrant
            {
                AboutYou = aboutYou,
            };

            cmd.Execute();

            var createdForm = Repository.Query<BestStartGrant>().ToList().FirstOrDefault();
            createdForm.Should().NotBeNull("form should be in database");
            createdForm.AboutYou.FirstName.Should().Be("unit test");
        }
    }
}
