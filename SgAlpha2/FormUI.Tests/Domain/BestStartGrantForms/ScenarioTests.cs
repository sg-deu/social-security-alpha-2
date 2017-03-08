using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class ScenarioTests : DomainTest
    {
        [Test]
        public void AgedUnder16()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next.Section.Should().Be(Sections.Consent);

            var consent = ConsentBuilder.NewValid();
            next = new AddConsent { FormId = formId, Consent = consent }.Execute();

            next.Section.Should().Be(Sections.ApplicantDetails);

            var applicantDetails = ApplicantDetailsBuilder.NewValid().Under16(TestNowUtc.Value);
            next = new AddApplicantDetails { FormId = formId, ApplicantDetails = applicantDetails }.Execute();

            next.Section.Should().Be(Sections.ExpectedChildren);

            var expectedChildren = ExpectedChildrenBuilder.NewValid();
            next = new AddExpectedChildren { FormId = formId, ExpectedChildren = expectedChildren }.Execute();

            next.Section.Should().Be(Sections.ExistingChildren);

            var existingChildren = ExistingChildrenBuilder.NewValid();
            next = new AddExistingChildren { FormId = formId, ExistingChildren = existingChildren }.Execute();

            next.Section.Should().Be(Sections.GuardianDetails1, "under 16 should be prompted for their parent's/guardian's details");

            var guardianDetails = GuardianDetailsBuilder.NewValid(Part.Part1);
            next = new AddGuardianDetails { FormId = formId, Part = Part.Part1, GuardianDetails = guardianDetails }.Execute();

            next.Section.Should().Be(Sections.GuardianDetails2);

            guardianDetails = GuardianDetailsBuilder.NewValid(Part.Part2);
            next = new AddGuardianDetails { FormId = formId, Part = Part.Part2, GuardianDetails = guardianDetails }.Execute();

            // under 16 should not be prompted for applicant benefits
            next.Section.Should().Be(Sections.HealthProfessional, "under 16 should not be prompted for applicant benefits");

            var healthProfessional = HealthProfessionalBuilder.NewValid();
            next = new AddHealthProfessional { FormId = formId, HealthProfessional = healthProfessional }.Execute();

            next.Section.Should().Be(Sections.PaymentDetails);

            var paymentDetails = PaymentDetailsBuilder.NewValid();
            next = new AddPaymentDetails { FormId = formId, PaymentDetails = paymentDetails }.Execute();

            var declaration = DeclarationBuilder.NewValid();
            next = new AddDeclaration { FormId = formId, Declaration = declaration }.Execute();

            next.Section.Should().BeNull();
        }
    }
}
