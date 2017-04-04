using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddPaymentDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresPaymentDetails()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .With(f => f.ExistingPaymentDetails, PaymentDetailsBuilder.NewValid())
                .Insert();

            existingForm.PaymentDetails.Should().BeNull("no data stored before executing command");

            var cmd = new AddPaymentDetails
            {
                FormId = "form123",
                PaymentDetails = PaymentDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.PaymentDetails.Should().NotBeNull();
            updatedForm.PaymentDetails.AccountNumber.Should().Be(cmd.PaymentDetails.AccountNumber);
        }
    }
}
