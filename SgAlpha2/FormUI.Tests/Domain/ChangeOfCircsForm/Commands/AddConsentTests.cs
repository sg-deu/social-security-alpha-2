using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddConsentTests : DomainTest
    {
        [Test]
        public void Execute_StoresConsent()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .Insert();

            existingForm.Consent.Should().BeNull("no data stored before executing command");

            var cmd = new AddConsent
            {
                FormId = "form123",
                Consent = ConsentBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Consent.Should().NotBeNull();
            updatedForm.Consent.AgreedToConsent.Should().Be(cmd.Consent.AgreedToConsent);
        }
    }
}
