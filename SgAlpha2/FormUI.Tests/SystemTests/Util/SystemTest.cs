using System;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Util
{
    [TestFixture]
    public abstract class SystemTest
    {
        protected BrowserApp App { get; set; }

        [SetUp]
        protected virtual void SetUp()
        {
            Console.WriteLine("Started {0}", TestContext.CurrentContext.Test.FullName);
            App = BrowserApp.Instance();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            Console.WriteLine("Completed {0} Status={1}\n",
                TestContext.CurrentContext.Test.FullName,
                TestContext.CurrentContext.Result.State);
        }
    }
}
