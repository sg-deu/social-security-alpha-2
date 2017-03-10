using System;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public abstract class DomainTest : AbstractTest
    {
        protected LocalRepository   Repository;
        protected DateTime?         TestNowUtc;

        [SetUp]
        protected virtual void SetUp()
        {
            Repository = LocalRepository.New().Register();
            DomainRegistry.ValidationContext = new ValidationContext(true);
            DomainRegistry.NowUtc = () => TestNowUtc ?? DateTime.UtcNow;
            TestNowUtc = new DateTime(2015, 07, 16, 12, 00, 00);
        }

        public override void TearDown()
        {
            base.TearDown();
            using (Repository) { }
        }

        protected void ShouldBeValid(Action action)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            Assert.DoesNotThrow(() => action());
        }

        protected void ShouldBeInvalid(Action action)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            Assert.Throws<DomainException>(() => action());
        }
    }
}
