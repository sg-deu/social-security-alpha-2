using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [SetUp]
        public void SetUp()
        {
            WebTest.SetUpWebHost();
        }

        [TearDown]
        public void TearDown()
        {
            WebTest.TearDownWebHost();
        }
    }
}
