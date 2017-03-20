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
                .WithCompletedSections()
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
    }
}
