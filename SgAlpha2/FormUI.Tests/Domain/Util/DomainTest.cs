using System;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public abstract class DomainTest
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

        [TearDown]
        protected virtual void TearDown()
        {
            using (Repository) { }
        }
    }
}
