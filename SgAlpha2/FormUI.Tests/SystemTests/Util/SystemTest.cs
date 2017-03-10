using System;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Util
{
    [TestFixture]
    public abstract class SystemTest : AbstractTest
    {
        protected BrowserApp App { get; set; }

        [SetUp]
        protected virtual void SetUp()
        {
            if (TestRegistry.TestHasFailed)
                Assert.Ignore("Ignored system test after test failure");

            Console.WriteLine("Started {0}", TestContext.CurrentContext.Test.FullName);
            App = BrowserApp.Instance();
            Db(r => r.DeleteAllDocuments());
        }

        public override void TearDown()
        {
            base.TearDown();

            Console.WriteLine("Completed {0} Status={1}\n",
                TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.State);
        }

        protected void Db(Action<LocalRepository> action)
        {
            Wait.For(null, () =>
            {
                using (var repository = LocalRepository.New(deleteAllDocuments: false))
                {
                    action(repository);
                }
            });
        }
    }
}
