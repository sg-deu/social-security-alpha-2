using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Queries
{
    [TestFixture]
    public class FindLatestApplicationTests : DomainTest
    {
        [Test]
        public void Find_OnlyReturnLatest()
        {
            var userId = "existing.user@known.com";
            var existingForm1 = new BestStartGrantBuilder("form1").WithCompletedSections().With(f => f.UserId, userId).With(f => f.Completed, new DateTime(2008, 07, 06)).Insert();
            var existingForm2 = new BestStartGrantBuilder("form2").WithCompletedSections().With(f => f.UserId, userId).With(f => f.Completed, new DateTime(2009, 07, 06)).Insert();
            var existingForm3 = new BestStartGrantBuilder("form3").WithCompletedSections().With(f => f.UserId, userId).With(f => f.Completed, new DateTime(2007, 07, 06)).Insert();

            var query = new FindLatestApplication
            {
                UserId = userId,
            };

            var detail = query.Find();

            detail.Should().NotBeNull();
            detail.Id.Should().Be(existingForm2.Id);
        }

        [Test]
        public void Find_OnlyReturnsCompleted()
        {
            var userId = "existing.user@known.com";

            var existingForm1 = new BestStartGrantBuilder("form1")
                .WithCompletedSections(markAsCompleted: false)
                .With(f => f.UserId, userId)
                .With(f => f.Declaration, null)
                .Insert();

            var query = new FindLatestApplication
            {
                UserId = userId,
            };

            var detail = query.Find();

            detail.Should().BeNull();
        }

        [Test]
        public void Find_OnlyReturnsUsersForms()
        {
            var userId = "existing.user@known.com";

            var existingForm1 = new BestStartGrantBuilder("form1")
                .WithCompletedSections()
                .With(f => f.UserId, "different.user@known.com")
                .With(f => f.Completed, new DateTime(2008, 07, 06))
                .Insert();

            var query = new FindLatestApplication
            {
                UserId = userId,
            };

            var detail = query.Find();

            detail.Should().BeNull();
        }
    }
}
