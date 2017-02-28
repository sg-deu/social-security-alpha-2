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
            var consent = ConsentBuilder.NewValid(m =>
                m.AgreedToConsent = true);

            var cmd = new StartBestStartGrant
            {
                Consent = consent,
            };

            var id = cmd.Execute();

            var createdForm = Repository.Query<BestStartGrant>().ToList().FirstOrDefault();
            createdForm.Should().NotBeNull("form should be in database");
            createdForm.Consent.AgreedToConsent.Should().BeTrue();

            createdForm.Id.Should().Be(id);
        }
    }
}
