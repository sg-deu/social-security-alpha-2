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
            form.TypeText(m => m.Title, "system test Title");
            form.TypeText(m => m.FirstName, "system test FirstName");
            form.TypeText(m => m.OtherNames, "system test OtherNames");
            form.TypeText(m => m.SurnameOrFamilyName, "system test FamilyName");
            form.TypeDate(m => m.DateOfBirth, "01", "02", "2003");
            form.TypeText(m => m.NationalInsuranceNumber, "AB123456C");
            form.TypeText(m => m.CurrentAddress.Street1, "system test ca.Street1");
            form.TypeText(m => m.CurrentAddress.Street2, "system test ca.Street2");
            form.TypeText(m => m.CurrentAddress.TownOrCity, "system test ca.TownOrCity");
            form.TypeText(m => m.CurrentAddress.Postcode, "system test ca.Postcode");
            form.TypeDate(m => m.CurrentAddress.DateMovedIn, "02", "03", "2004");
            form.SelectRadio(m => m.CurrentAddressStatus, AddressStatus.Permanent);
            form.SelectRadio(m => m.ContactPreference, ContactPreference.Email);
            form.TypeText(m => m.EmailAddress, "test.system@system.test");
        }
    }
}
