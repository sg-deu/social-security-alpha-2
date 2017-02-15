using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Bsg
{
    [TestFixture]
    public class BsgSystemTests : SystemTest
    {
        [Test]
        [Explicit("Marked explicit until we get this running in both Chrome and PhantomJS")]
        public void ProcessApplication()
        {
            App.GoTo("http://www.google.com");
        }
    }
}
