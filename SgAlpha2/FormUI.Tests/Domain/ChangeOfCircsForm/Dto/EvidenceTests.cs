using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
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
