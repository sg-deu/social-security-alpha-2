using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddPartnerBenefitsTests : DomainTest
    {
        [Test]
        public void Execute_StoresBenefitsDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.PartnerBenefits.Should().BeNull("no data stored before executing command");

            var cmd = new AddPartnerBenefits
            {
                FormId = "form123",
                PartnerBenefits = BenefitsBuilder.NewWithBenefit(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.PartnerBenefits.Should().NotBeNull();
            updatedForm.PartnerBenefits.HasIncomeSupport.Should().Be(cmd.PartnerBenefits.HasIncomeSupport);
        }
    }
}
