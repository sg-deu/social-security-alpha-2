using FormUI.Domain.BestStartGrantForms;
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
            BsgFacade.Init();
            Repository = LocalRepository.New().Register();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            using (Repository) { }
        }
    }
}
