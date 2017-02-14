using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class FacadeTests : DomainTest
    {
        [Test]
        public void Start_CreatesForm()
        {
            var aboutYou = AboutYouBuilder.NewValidAboutYou(m =>
                m.FirstName = "unit test");

            BsgFacade.Start(aboutYou);

            var createdForm = Repository.Query<BestStartGrant>().ToList().FirstOrDefault();
            createdForm.Should().NotBeNull("form should be in database");
            createdForm.AboutYou.FirstName.Should().Be("unit test");
        }
    }
}
