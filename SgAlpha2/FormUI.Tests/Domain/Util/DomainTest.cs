using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public abstract class DomainTest
    {
        protected LocalRepository Repository;

        [SetUp]
        protected virtual void SetUp()
        {
            Repository = LocalRepository.New().Register();
            DomainRegistry.ValidationContext = new ValidationContext(true);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            using (Repository) { }
        }
    }
}
