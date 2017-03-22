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
        public void ChangeDetails()
        {
            var userId = "test.identity@test.site";
            new BestStartGrantBuilder("existingForm").PreviousApplicationFor(userId).Insert();

            var next = new StartChangeOfCircs().Execute();
            var formId = next.Id;

            next = AddConsent(next);
            next = AddIdentity(next, userId);
            next = AddApplicantDetails(next);

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

        private NextSection AddApplicantDetails(NextSection current, Action<ApplicantDetails> mutator = null)
        {
            current.Section.Should().Be(Sections.ApplicantDetails);
            return NextSection(current.Section, () => new AddApplicantDetails { FormId = current.Id, ApplicantDetails = ApplicantDetailsBuilder.NewValid(mutator) }.Execute());
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
