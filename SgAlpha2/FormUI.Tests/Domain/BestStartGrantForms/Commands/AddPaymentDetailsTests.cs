using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddPaymentDetailsTests : DomainTest
    {
        [Test]
        public void Execute_StoresPaymentDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid())
                .With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid())
                .With(f => f.HealthProfessional, HealthProfessionalBuilder.NewValid())
                .Insert();

            existingForm.PaymentDetails.Should().BeNull("no data stored before executing command");

            var cmd = new AddPaymentDetails
            {
                FormId = "form123",
                PaymentDetails = PaymentDetailsBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.PaymentDetails.Should().NotBeNull();
            updatedForm.PaymentDetails.AccountNumber.Should().Be(cmd.PaymentDetails.AccountNumber);
        }
    }
}
