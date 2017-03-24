using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Queries;
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
        public void AgedOver25OnBenefits()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);
            next = AddApplicantBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void AgedOver25PartnerOnBenefits()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);
            next = AddApplicantBenefits(next, b => b.HasExistingBenefit = YesNoDk.No);
            next = AddPartnerBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);
            next = AddPartnerDetails1(next);
            next = AddPartnerDetails2(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void NoExistingChildren()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 0);
            next = AddApplicantBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void MultipleExistingChildren_OneNotKinshipCare()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 3, ec => ec.LastNotKinshipCare());
            next = AddApplicantBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void MultipleExistingChildren_AllKinshipCare()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 3, ec => ec.AllKinshipCare());

            next.Section.Should().Be(Sections.HealthProfessional, "where all existing children are kinship care, qualifying benefits are not required");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void MultipleExistingChildren_AllKinshipCare_AtLEastOneBabyExpected()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.ExpectedBabyCount(1));
            next = AddExistingChildren(next, 3, ec => ec.AllKinshipCare());

            next.Section.Should().Be(Sections.ApplicantBenefits, "when there is an expected child, the expected child is not expected to be kinship care");

            next = AddApplicantBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void CareLeaverUnder25()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Under25CareLeaver(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);

            next.Section.Should().Be(Sections.HealthProfessional, "when under 25, and was previously in care, qualifying benefits are not required");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void AgedUnder16()
        {
            // under 16 is automatically eligible, but need legal parent/guardian details
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
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

        [Test]
        public void Aged16()
        {
            // 16/17 is automatically eligible, but no need gather legal parent/guardian details
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Aged16(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);

            next.Section.Should().Be(Sections.HealthProfessional, "under 16 should not be prompted for guardian details or applicant benefits");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void Aged18PartOfGuardianBenefits()
        {
            // 18, living with parents, and guardian's partner has a qualifying benefit
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.PartOfGuardianBenefits(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);

            next.Section.Should().Be(Sections.GuardianBenefits, "18/19 should confirm the benefits their guardian's partner is on");
            next = AddGuardianBenefits(next, b => b.HasExistingBenefit = YesNoDk.Yes);

            next.Section.Should().Be(Sections.GuardianDetails1, "when guardian has a qualifying benefit, should skip guardian's partner's benefits");

            next = AddGuardianDetails1(next);
            next = AddGuardianDetails2(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void Aged18PartOfGuardianPartnerBenefits()
        {
            // 18, living with parents, and guardian's partner has a qualifying benefit
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.PartOfGuardianBenefits(TestNowUtc.Value));
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);

            next.Section.Should().Be(Sections.GuardianBenefits, "18/19 should confirm the benefits their guardian's partner is on");
            next = AddGuardianBenefits(next, b => b.HasExistingBenefit = YesNoDk.No);

            next.Section.Should().Be(Sections.GuardianPartnerBenefits, "when guardian does not have a qualifying benefit, should be prompted for the guardian's partner's benefits");
            next = AddGuardianPartnerBenefits(next);

            next = AddGuardianPartnerDetails1(next);
            next = AddGuardianPartnerDetails2(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void Aged18PartOfGuardianBenefits_AllChildrenKinshipCare()
        {
            // 18, living with parents, and all children kinship care
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.PartOfGuardianBenefits(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 3, ec => ec.AllKinshipCare());

            next.Section.Should().Be(Sections.HealthProfessional, "when all children kinship care, should skip all benefits");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        #region command execution helpers

        private NextSection AddConsent(NextSection current, Action<Consent> mutator = null)
        {
            current.Section.Should().Be(Sections.Consent);
            return NextSection(current.Section, () => new AddConsent { FormId = current.Id, Consent = ConsentBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddUKVerify(NextSection current, Action<UKVerify> mutator = null)
        {
            current.Section.Should().Be(Sections.UKVerify);
            return NextSection(current.Section, () => new AddUKVerify { FormId = current.Id, UKVerify = UKVerifyBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddApplicantDetails(NextSection current, Action<ApplicantDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantDetails);
            return NextSection(current.Section, () => new AddApplicantDetails { FormId = current.Id, ApplicantDetails = ApplicantDetailsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddExpectedChildren(NextSection current, Action<ExpectedChildren> mutator = null)
        {
            current.Section.Should().Be(Sections.ExpectedChildren);
            return NextSection(current.Section, () => new AddExpectedChildren { FormId = current.Id, ExpectedChildren = ExpectedChildrenBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddExistingChildren(NextSection current, int childCount = 2, Action<ExistingChildren> mutator = null)
        {
            current.Section.Should().Be(Sections.ExistingChildren);
            return NextSection(current.Section, () => new AddExistingChildren { FormId = current.Id, ExistingChildren = ExistingChildrenBuilder.NewValid(childCount, mutator) }.Execute());
        }

        private NextSection AddApplicantBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantBenefits);
            return NextSection(current.Section, () => new AddApplicantBenefits { FormId = current.Id, ApplicantBenefits = BenefitsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddPartnerBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.PartnerBenefits);
            return NextSection(current.Section, () => new AddPartnerBenefits { FormId = current.Id, PartnerBenefits = BenefitsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddGuardianBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianBenefits);
            return NextSection(current.Section, () => new AddGuardianBenefits { FormId = current.Id, GuardianBenefits = BenefitsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddGuardianPartnerBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianPartnerBenefits);
            return NextSection(current.Section, () => new AddGuardianPartnerBenefits { FormId = current.Id, GuardianPartnerBenefits = BenefitsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddPartnerDetails1(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.PartnerDetails1);
            return NextSection(current.Section, () => new AddPartnerDetails { FormId = current.Id, Part = Part.Part1, PartnerDetails = RelationDetailsBuilder.NewValid(Part.Part1, mutator) }.Execute());
        }

        private NextSection AddPartnerDetails2(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.PartnerDetails2);
            return NextSection(current.Section, () => new AddPartnerDetails { FormId = current.Id, Part = Part.Part2, PartnerDetails = RelationDetailsBuilder.NewValid(Part.Part2, mutator) }.Execute());
        }

        private NextSection AddGuardianDetails1(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianDetails1);
            return NextSection(current.Section, () => new AddGuardianDetails { FormId = current.Id, Part = Part.Part1, GuardianDetails = RelationDetailsBuilder.NewValid(Part.Part1, mutator) }.Execute());
        }

        private NextSection AddGuardianDetails2(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianDetails2);
            return NextSection(current.Section, () => new AddGuardianDetails { FormId = current.Id, Part = Part.Part2, GuardianDetails = RelationDetailsBuilder.NewValid(Part.Part2, mutator) }.Execute());
        }

        private NextSection AddGuardianPartnerDetails1(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianPartnerDetails1);
            return NextSection(current.Section, () => new AddGuardianPartnerDetails { FormId = current.Id, Part = Part.Part1, GuardianPartnerDetails = RelationDetailsBuilder.NewValid(Part.Part1, mutator) }.Execute());
        }

        private NextSection AddGuardianPartnerDetails2(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianPartnerDetails2);
            return NextSection(current.Section, () => new AddGuardianPartnerDetails { FormId = current.Id, Part = Part.Part2, GuardianPartnerDetails = RelationDetailsBuilder.NewValid(Part.Part2, mutator) }.Execute());
        }

        private NextSection AddHealthProfessional(NextSection current, Action<HealthProfessional> mutator = null)
        {
            current.Section.Should().Be(Sections.HealthProfessional);
            return NextSection(current.Section, () => new AddHealthProfessional { FormId = current.Id, HealthProfessional = HealthProfessionalBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddPaymentDetails(NextSection current, Action<PaymentDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.PaymentDetails);
            return NextSection(current.Section, () => new AddPaymentDetails { FormId = current.Id, PaymentDetails = PaymentDetailsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddDeclaration(NextSection current, Action<Declaration> mutator = null)
        {
            current.Section.Should().Be(Sections.Declaration);
            return NextSection(current.Section, () => new AddDeclaration { FormId = current.Id, Declaration = DeclarationBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection NextSection(Sections? currentSection, Func<NextSection> nextSection)
        {
            var next = nextSection();

            if (next.Section.HasValue)
            {
                var detail = new FindBsgSection { FormId = next.Id, Section = next.Section.Value }.Find();
                detail.PreviousSection.Should().Be(currentSection);
                detail.IsFinalSection.Should().Be(Navigation.Order.Last() == next.Section.Value, "Expected {0} to be the final section", next.Section.Value);
            }

            return next;
        }

        #endregion
    }
}
