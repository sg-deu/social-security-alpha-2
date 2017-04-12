using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ScenarioTests : DomainTest
    {
        [Test]
        public void ChangeAll()
        {
            var userId = "test.identity@test.site";
            new BestStartGrantBuilder("existingForm").PreviousApplicationFor(userId).Insert();

            var next = new StartChangeOfCircs().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddIdentity(next, userId);
            next = AddOptions(next);
            next = AddApplicantDetails(next);
            next = AddExpectedChildren(next);
            next = AddHealthProfessional(next);
            next = AddPaymentDetails(next);
            next = AddEvidence(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        [Test]
        public void ChangeOnlyOther()
        {
            var userId = "test.identity@test.site";
            new BestStartGrantBuilder("existingForm").PreviousApplicationFor(userId).Insert();

            var next = new StartChangeOfCircs().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddIdentity(next, userId);
            next = AddOptions(next, o =>
            {
                o.AllUnselected();
                o.Other = true;
                o.OtherDetails = "some details";
            });
            next = AddEvidence(next);
            next = AddDeclaration(next);

            next.Section.Should().BeNull();
        }

        #region command execution helpers

        private NextSection AddConsent(NextSection current, Action<Consent> mutator = null)
        {
            current.Section.Should().Be(Sections.Consent);
            return NextSection(current.Section, () => new AddConsent { FormId = current.Id, Consent = ConsentBuilder.NewValid(mutator) }.Execute());
        }

        private NextSection AddIdentity(NextSection current, string userId)
        {
            current.Section.Should().Be(Sections.Identity);
            return NextSection(current.Section, () => new AddIdentity { FormId = current.Id, Identity = userId }.Execute());
        }

        private NextSection AddOptions(NextSection current, Action<Options> mutator = null)
        {
            current.Section.Should().Be(Sections.Options);
            return NextSection(current.Section, () => new AddOptions { FormId = current.Id, Options = OptionsBuilder.NewValid(mutator) }.Execute());
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

        private NextSection AddEvidence(NextSection current, Action<Evidence> mutator = null)
        {
            current.Section.Should().Be(Sections.Evidence);
            return NextSection(current.Section, () => new AddEvidence { FormId = current.Id, Evidence = EvidenceBuilder.NewValid(mutator) }.Execute());
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
                var detail = new FindCocSection { FormId = next.Id, Section = next.Section.Value }.Find();
                detail.PreviousSection.Should().Be(currentSection);
                detail.IsFinalSection.Should().Be(Navigation.Order.Last() == next.Section.Value, "Expected {0} to be the final section", next.Section.Value);
            }

            return next;
        }

        #endregion
    }
}
