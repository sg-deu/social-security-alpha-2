using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class ValidateTests : DomainTest
    {
        [Test]
        public void Execute_WhenValid_ReturnsTrue()
        {
            var cmd = new Validate
            {
                ExpectedChildren = ExpectedChildrenBuilder.NewValid(),
                HealthProfessional = HealthProfessionalBuilder.NewValid(),
            };

            var valid = cmd.Execute();

            valid.Should().BeTrue();
        }

        [Test]
        public void Execute_WhenNotSupplied_DoesNotThrow()
        {
            var cmd = new Validate();

            var valid = cmd.Execute();

            valid.Should().BeTrue();
        }

        [Test]
        public void Execute_ValidatesExpectedChildren()
        {
            var cmd = new Validate
            {
                ExpectedChildren = ExpectedChildrenBuilder.NewValid(m => m.Invalid()),
            };

            Assert.Throws<DomainException>(() => cmd.Execute());
        }

        [Test]
        public void Execute_ValidatesHealthProfessional()
        {
            var cmd = new Validate
            {
                HealthProfessional = HealthProfessionalBuilder.NewValid(m => m.Invalid()),
            };

            Assert.Throws<DomainException>(() => cmd.Execute());
        }
    }
}
