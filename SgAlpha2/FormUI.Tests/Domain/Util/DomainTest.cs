using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace FormUI.Tests.Domain.Util
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public abstract class DomainTest
    {
        protected LocalRepository Repository;

        [SetUp]
        protected virtual void SetUp()
        {
            Repository = LocalRepository.New().Register();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            using (Repository) { }
        }
    }
}
