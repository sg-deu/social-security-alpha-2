using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddConsentTests : DomainTest
    {
        [Test]
        public void Execute_StoresConsent()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.Consent.Should().BeNull("no data stored before executing command");

            var cmd = new AddConsent
            {
                FormId = "form123",
                Consent = ConsentBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.Consent.Should().NotBeNull();
            updatedForm.Consent.AgreedToConsent.Should().Be(cmd.Consent.AgreedToConsent);
        }
    }
}
