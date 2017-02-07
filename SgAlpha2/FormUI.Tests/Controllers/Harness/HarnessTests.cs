using FormUI.Controllers.Harness;
using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Harness
{
    [TestFixture]
    public class HarnessTests : WebTest
    {
        [Test]
        public void Index_GET()
        {
            VerifyView(HarnessActions.Index());
        }

        [Test]
        public void InputText_GET()
        {
            VerifyView(HarnessActions.InputText());
        }

        [Test]
        public void InputDate_GET()
        {
            VerifyView(HarnessActions.InputDate());
        }
    }
}
