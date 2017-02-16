using FormUI.Controllers.Bsg;
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
            App.GoTo(BsgActions.Overview());
            App.VerifyCanSeeText("Overview");
        }
    }
}
