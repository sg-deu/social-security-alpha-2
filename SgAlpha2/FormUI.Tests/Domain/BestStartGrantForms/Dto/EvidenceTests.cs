using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class EvidenceTests : AbstractTest
    {
        [Test]
        public void CopyTo_CoversAllDetails()
        {
            var src = EvidenceBuilder.NewValid();
            var dest = new Evidence();

            src.CopyTo(dest);

            dest.ShouldBeEquivalentTo(src);
        }
    }
}
