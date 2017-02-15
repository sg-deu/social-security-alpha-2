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
            var runHeadless = RunHeadless();
            App = new BrowserApp(runHeadless);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            using (App) { }
        }

        private bool RunHeadless()
        {
            return true;
        }
    }
}
