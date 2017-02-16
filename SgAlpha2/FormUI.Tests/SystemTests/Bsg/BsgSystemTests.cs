using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Bsg
{
    [TestFixture]
    public class BsgSystemTests : SystemTest
    {
        [Test]
        public void CompleteApplication()
        {
            App.GoTo(BsgActions.Overview());
            App.VerifyCanSeeText("Overview");

            App.ClickLinkButton("Next");
            App.VerifyCanSeeText("About You ...");

            var form = App.FormFormModel<AboutYou>();
            form.TypeText(m => m.Title, "system test title");
        }
    }
}
