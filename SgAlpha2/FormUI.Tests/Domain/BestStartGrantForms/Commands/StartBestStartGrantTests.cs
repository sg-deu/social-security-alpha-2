using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class StartBestStartGrantTests : DomainTest
    {
        [Test]
        public void Execute_CreatesForm()
        {
            var applicantDetails = ApplicantDetailsBuilder.NewValid(m =>
                m.FirstName = "unit test");

            var cmd = new StartBestStartGrant
            {
                ApplicantDetails = applicantDetails,
            };

            var id = cmd.Execute();

            var createdForm = Repository.Query<BestStartGrant>().ToList().FirstOrDefault();
            createdForm.Should().NotBeNull("form should be in database");
            createdForm.ApplicantDetails.FirstName.Should().Be("unit test");

            createdForm.Id.Should().Be(id);
        }
    }
}
