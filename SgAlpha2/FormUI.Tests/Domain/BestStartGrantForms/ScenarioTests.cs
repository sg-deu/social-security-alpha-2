using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
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

            next = AddConsent(next);
            next = AddApplicantDetails(next, ad => ad.Under16(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);

            next.Section.Should().Be(Sections.GuardianDetails1, "under 16 should be prompted for their parent's/guardian's details");

            next = AddGuardianDetails1(next);
            next = AddGuardianDetails2(next);

            next.Section.Should().Be(Sections.HealthProfessional, "under 16 should not be prompted for applicant benefits");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        #region command execution helpers

        private NextSection AddConsent(NextSection current, Action<Consent> mutator = null)
        {
            current.Section.Should().Be(Sections.Consent);
            return new AddConsent { FormId = current.Id, Consent = ConsentBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddApplicantDetails(NextSection current, Action<ApplicantDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantDetails);
            return new AddApplicantDetails { FormId = current.Id, ApplicantDetails = ApplicantDetailsBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddExpectedChildren(NextSection current, Action<ExpectedChildren> mutator = null)
        {
            current.Section.Should().Be(Sections.ExpectedChildren);
            return new AddExpectedChildren { FormId = current.Id, ExpectedChildren = ExpectedChildrenBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddExistingChildren(NextSection current, Action<ExistingChildren> mutator = null)
        {
            current.Section.Should().Be(Sections.ExistingChildren);
            return new AddExistingChildren { FormId = current.Id, ExistingChildren = ExistingChildrenBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddGuardianDetails1(NextSection current, Action<GuardianDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianDetails1);
            return new AddGuardianDetails { FormId = current.Id, Part = Part.Part1, GuardianDetails = GuardianDetailsBuilder.NewValid(Part.Part1, mutator) }.Execute();
        }

        private NextSection AddGuardianDetails2(NextSection current, Action<GuardianDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianDetails2);
            return new AddGuardianDetails { FormId = current.Id, Part = Part.Part2, GuardianDetails = GuardianDetailsBuilder.NewValid(Part.Part2, mutator) }.Execute();
        }

        private NextSection AddApplicantBenefits1(NextSection current, Action<ApplicantBenefits> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantBenefits1);
            return new AddApplicantBenefits { FormId = current.Id, Part = Part.Part1, ApplicantBenefits = ApplicantBenefitsBuilder.NewValid(Part.Part1, mutator) }.Execute();
        }

        private NextSection AddApplicantBenefits2(NextSection current, Action<ApplicantBenefits> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantBenefits2);
            return new AddApplicantBenefits { FormId = current.Id, Part = Part.Part2, ApplicantBenefits = ApplicantBenefitsBuilder.NewValid(Part.Part2, mutator) }.Execute();
        }

        private NextSection AddHealthProfessional(NextSection current, Action<HealthProfessional> mutator = null)
        {
            current.Section.Should().Be(Sections.HealthProfessional);
            return new AddHealthProfessional { FormId = current.Id, HealthProfessional = HealthProfessionalBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddPaymentDetails(NextSection current, Action<PaymentDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.PaymentDetails);
            return new AddPaymentDetails { FormId = current.Id, PaymentDetails = PaymentDetailsBuilder.NewValid(mutator) }.Execute();
        }

        private NextSection AddDeclaration(NextSection current, Action<Declaration> mutator = null)
        {
            current.Section.Should().Be(Sections.Declaration);
            return new AddDeclaration { FormId = current.Id, Declaration = DeclarationBuilder.NewValid(mutator) }.Execute();
        }

        #endregion
    }
}
