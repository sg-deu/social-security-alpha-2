using System.Threading;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Bsg
{
    [TestFixture]
    public class BsgSystemTests : SystemTest
    {
        [Test]
        public void VerifyWeCanOpenBrowser()
        {
            App.GoTo("http://localhost:54077/");
            Thread.Sleep(3000);
        }
    }
}
