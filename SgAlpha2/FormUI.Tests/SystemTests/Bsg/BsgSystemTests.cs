using System.Threading;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Bsg
{
    [TestFixture]
    public class BsgSystemTests : SystemTest
    {
        [Test]
        [Explicit("Marked explicit until we get this running in both Chrome and PhantomJS")]
        public void VerifyWeCanOpenBrowser()
        {
            App.GoTo("about:blank");
            Thread.Sleep(3000);
        }
    }
}
