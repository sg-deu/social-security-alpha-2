using System;
using System.Linq;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
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

            {
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

            App.Submit();

            {
                var form = App.FormFormModel<ExpectedChildren>();
                form.TypeDate(m => m.ExpectancyDate, "02", "03", "2004");
                form.TypeText(m => m.ExpectedBabyCount, "3");
            }

            App.Submit();

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                doc.AboutYou.Title.Should().Be("system test Title");
                doc.AboutYou.FirstName.Should().Be("system test FirstName");
                doc.AboutYou.OtherNames.Should().Be("system test OtherNames");
                doc.AboutYou.SurnameOrFamilyName.Should().Be("system test FamilyName");
                doc.AboutYou.DateOfBirth.Should().Be(new DateTime(2003, 02, 01));
                doc.AboutYou.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
                doc.AboutYou.CurrentAddress.Street1.Should().Be("system test ca.Street1");
                doc.AboutYou.CurrentAddress.Street2.Should().Be("system test ca.Street2");
                doc.AboutYou.CurrentAddress.TownOrCity.Should().Be("system test ca.TownOrCity");
                doc.AboutYou.CurrentAddress.Postcode.Should().Be("system test ca.Postcode");
                doc.AboutYou.CurrentAddress.DateMovedIn.Should().Be(new DateTime(2004, 03, 02));
                doc.AboutYou.CurrentAddressStatus.Should().Be(AddressStatus.Permanent);
                doc.AboutYou.ContactPreference.Should().Be( ContactPreference.Email);
                doc.AboutYou.EmailAddress.Should().Be("test.system@system.test");

                doc.ExpectedChildren.ExpectancyDate.Should().Be(new DateTime(2004, 03, 02));
                doc.ExpectedChildren.ExpectedBabyCount.Should().Be(3);
            });
        }
    }
}
