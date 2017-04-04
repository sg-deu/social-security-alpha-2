using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Queries;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Queries
{
    [TestFixture]
    public class FindCocSectionTests : DomainTest
    {
        [Test]
        public void Find_PopulatesDetail()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .WithCompletedSections(markAsCompleted: false)
                .Insert();

            var query = new FindCocSection
            {
                FormId = "form123",
                Section = Sections.Consent,
            };

            var detail = query.Find();

            var expectedDetail = new CocDetail { PreviousSection = null };
            ChangeOfCircsBuilder.CopySectionsFrom(existingForm, expectedDetail);

            detail.ShouldBeEquivalentTo(expectedDetail);
        }

        [Test]
        public void Find_PopulatesExistingDetail_IfCurrentDetailDoesNotExist()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .WithCompletedSections(excludeOptionalSections: true)
                .Insert();

            var query = new FindCocSection
            {
                FormId = "form123",
                Section = Sections.Consent,
            };

            var details = query.Find();

            var expectedDetail = new CocDetail { PreviousSection = null };
            ChangeOfCircsBuilder.CopySectionsFrom(existingForm, expectedDetail, useExisting: true);

            details.ShouldBeEquivalentTo(expectedDetail);
        }
    }
}
