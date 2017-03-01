using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddHealthProfessionalTests : DomainTest
    {
        [Test]
        public void Execute_StoresHealthProfessionalDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid())
                .With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid())
                .Insert();

            existingForm.HealthProfessional.Should().BeNull("no data stored before executing command");

            var cmd = new AddHealthProfessional
            {
                FormId = "form123",
                HealthProfessional = HealthProfessionalBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.HealthProfessional.Should().NotBeNull();
            updatedForm.HealthProfessional.Pin.Should().Be(cmd.HealthProfessional.Pin);
        }
    }
}
