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
        private bool _gotToEnd;

        protected override void SetUp()
        {
            _gotToEnd = false;
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            _gotToEnd.Should().BeTrue("should VerifyIneligible() or VerifyComplete()");
        }

        [Test]
        public void NoChildren_Ineligible()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 0);

            VerifyIneligible(next);
        }

        [Test]
        public void NoBenefits_Ineligible()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next);
            next = AddExpectedChildren(next);
            next = AddExistingChildren(next);
            next = AddApplicantBenefits(next, b => b.None());
            next = AddPartnerBenefits(next, b => b.None());

            VerifyIneligible(next);
        }

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
            next = AddApplicantBenefits(next, b => b.WithBenefit());
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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
            next = AddApplicantBenefits(next, b => b.None());
            next = AddPartnerBenefits(next, b => b.WithBenefit());
            next = AddPartnerDetails(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
        }

        [Test]
        public void OneExistingChild()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.NoBabyExpected());
            next = AddExistingChildren(next, 1);
            next = AddApplicantBenefits(next, b => b.WithBenefit());
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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
            next = AddApplicantBenefits(next, b => b.WithBenefit());
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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

            VerifyComplete(next);
        }

        [Test]
        public void MultipleExistingChildren_AllKinshipCare_AtLeastOneBabyExpected()
        {
            var next = new StartBestStartGrant().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddUKVerify(next);
            next = AddApplicantDetails(next, ad => ad.Over25(TestNowUtc.Value));
            next = AddExpectedChildren(next, ec => ec.ExpectedBabyCount(1));
            next = AddExistingChildren(next, 3, ec => ec.AllKinshipCare());

            next.Section.Should().Be(Sections.ApplicantBenefits, "when there is an expected child, the expected child is not expected to be kinship care");

            next = AddApplicantBenefits(next, b => b.WithBenefit());
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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

            VerifyComplete(next);
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

            next.Section.Should().Be(Sections.GuardianDetails, "under 16 should be prompted for their parent's/guardian's details");

            next = AddGuardianDetails(next);

            next.Section.Should().Be(Sections.HealthProfessional, "under 16 should not be prompted for applicant benefits");

            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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

            VerifyComplete(next);
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
            next = AddGuardianBenefits(next, b => b.WithBenefit());

            next.Section.Should().Be(Sections.GuardianDetails, "when guardian has a qualifying benefit, should skip guardian's partner's benefits");

            next = AddGuardianDetails(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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
            next = AddGuardianBenefits(next, b => b.None());

            next.Section.Should().Be(Sections.GuardianPartnerBenefits, "when guardian does not have a qualifying benefit, should be prompted for the guardian's partner's benefits");
            next = AddGuardianPartnerBenefits(next);

            next = AddGuardianPartnerDetails(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddDeclaration(next);

            VerifyComplete(next);
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

            VerifyComplete(next);
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
            return NextSection(current.Section, () => new AddApplicantBenefits { FormId = current.Id, ApplicantBenefits = BenefitsBuilder.NewWithBenefit(mutator) }.Execute());
        }

        private NextSection AddPartnerBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.PartnerBenefits);
            return NextSection(current.Section, () => new AddPartnerBenefits { FormId = current.Id, PartnerBenefits = BenefitsBuilder.NewWithBenefit(mutator) }.Execute());
        }

        private NextSection AddGuardianBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianBenefits);
            return NextSection(current.Section, () => new AddGuardianBenefits { FormId = current.Id, GuardianBenefits = BenefitsBuilder.NewWithBenefit(mutator) }.Execute());
        }

        private NextSection AddGuardianPartnerBenefits(NextSection current, Action<Benefits> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianPartnerBenefits);
            return NextSection(current.Section, () => new AddGuardianPartnerBenefits { FormId = current.Id, GuardianPartnerBenefits = BenefitsBuilder.NewWithBenefit(mutator) }.Execute());
        }

        private NextSection AddPartnerDetails(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.PartnerDetails);
            return NextSection(current.Section, () => new AddPartnerDetails { FormId = current.Id, PartnerDetails = RelationDetailsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddGuardianDetails(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianDetails);
            return NextSection(current.Section, () => new AddGuardianDetails { FormId = current.Id, GuardianDetails = RelationDetailsBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddGuardianPartnerDetails(NextSection current, Action<RelationDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.GuardianPartnerDetails);
            return NextSection(current.Section, () => new AddGuardianPartnerDetails { FormId = current.Id, GuardianPartnerDetails = RelationDetailsBuilder.NewValid(mutator) }.Execute());
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
                next.Type.Should().Be(NextType.Section);
                var detail = new FindBsgSection { FormId = next.Id, Section = next.Section.Value }.Find();
                detail.PreviousSection.Should().Be(currentSection);
                detail.IsFinalSection.Should().Be(Navigation.Order.Last() == next.Section.Value, "Expected {0} to be the final section", next.Section.Value);
            }

            return next;
        }

        private void VerifyComplete(NextSection next)
        {
            next.Type.Should().Be(NextType.Complete);
            next.Section.Should().BeNull();
            _gotToEnd = true;
        }

        private void VerifyIneligible(NextSection next)
        {
            next.Type.Should().Be(NextType.Ineligible);
            next.Section.Should().BeNull();
            var form = Repository.Load<BestStartGrant>(next.Id);
            form.Completed.Should().BeNull("ineligible should not 'complete' the form");
            _gotToEnd = true;
        }

        #endregion
    }
}
