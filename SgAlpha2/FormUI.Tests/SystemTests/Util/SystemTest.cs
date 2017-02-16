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
            App = BrowserApp.Instance();
        }

        [TearDown]
        protected virtual void TearDown()
        {
        }
    }
}
