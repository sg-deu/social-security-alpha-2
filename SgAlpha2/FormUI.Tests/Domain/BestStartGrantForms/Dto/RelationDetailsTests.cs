using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Forms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class RelationDetailsTests : AbstractTest
    {
        [Test]
        public void CopyTo_CoversAllDetails()
        {
            var src = RelationDetailsBuilder.NewValid();
            var dest = new RelationDetails();

            src.CopyTo(dest);

            dest.Title.Should().Be(src.Title);

            src.CopyTo(dest);

            dest.ShouldBeEquivalentTo(src);
        }

        [Test]
        public void CopyTo_OnlyCopiesUninheritedAddress()
        {
            var src = RelationDetailsBuilder.NewValid(rd =>
            {
                rd.InheritAddress = true;
                rd.Address = AddressBuilder.NewValid();
            });

            var dest = new RelationDetails();

            src.CopyTo(dest);

            dest.InheritAddress.Should().BeTrue();
            dest.Address.Should().BeNull();
        }
    }
}
