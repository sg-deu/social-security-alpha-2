using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Queries
{
    [TestFixture]
    public class FindBsgSectionTests : DomainTest
    {
        [Test]
        public void Find_PopulatesDetail()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .WithCompletedSections()
                .Insert();

            var query = new FindBsgSection
            {
                FormId = "form123",
            };

            var detail = query.Find();

            detail.Consent.Should().NotBeNull();
        }
    }
}
