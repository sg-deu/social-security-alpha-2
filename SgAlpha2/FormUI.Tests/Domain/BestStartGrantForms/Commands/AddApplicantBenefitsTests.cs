using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddApplicantBenefitsTests : DomainTest
    {
        [Test]
        public void Execute_StoresBenefitsDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.ApplicantBenefits, ApplicantBenefitsBuilder.NewValid(Part.Part1))
                .Insert();

            existingForm.ApplicantBenefits.YouOrPartnerInvolvedInTradeDispute.Should().BeNull("no data stored before executing command");

            var cmd = new AddApplicantBenefits
            {
                FormId = "form123",
                Part = Part.Part2,
                ApplicantBenefits = ApplicantBenefitsBuilder.NewValid(Part.Part2),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.ApplicantBenefits.Should().NotBeNull();
            updatedForm.ApplicantBenefits.YouOrPartnerInvolvedInTradeDispute.Should().Be(cmd.ApplicantBenefits.YouOrPartnerInvolvedInTradeDispute);
        }
    }
}
