using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class GuardianDetailsTests
    {
        [Test]
        public void CopyTo_CoversAllDetails()
        {
            var src = GuardianDetailsBuilder.NewValid(Part.Part2);
            var dest = new GuardianDetails();

            src.CopyTo(dest, Part.Part1);

            dest.Title.Should().Be(src.Title);

            src.CopyTo(dest, Part.Part2);

            dest.ShouldBeEquivalentTo(src);
        }
    }
}
