using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Queries
{
    [TestFixture]
    public class FindApplicantDetailsConfigTests : DomainTest
    {
        [Test]
        public void Find_BothQuestionsAsked()
        {
            var query = new FindApplicantDetailsConfig
            {
                ApplicantDetails = ApplicantDetailsBuilder.NewValid(),
            };

            var detail = query.Find();

            detail.ShouldAskCareQuestion.Should().BeTrue();
            detail.ShouldAskEducationQuestion.Should().BeTrue();
        }

        [Test]
        public void Find_OneQuestionAsked()
        {
            var query = new FindApplicantDetailsConfig
            {
                ApplicantDetails = ApplicantDetailsBuilder.NewValid(ad => ad.DateOfBirth = TestNowUtc.Value.Date.AddYears(-21)),
            };

            var detail = query.Find();

            detail.ShouldAskCareQuestion.Should().BeTrue();
            detail.ShouldAskEducationQuestion.Should().BeFalse();
        }
    }
}
