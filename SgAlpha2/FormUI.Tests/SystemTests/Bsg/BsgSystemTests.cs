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

            App.Submit();
            App.VerifyCanSeeText("Consent");

            {

                var form = App.FormForModel<Consent>();
                form.Check(m => m.AgreedToConsent, true);
            }

            App.Submit();

            var dob = DateTime.Now.Date.AddYears(-19);

            {
                var form = App.FormForModel<ApplicantDetails>();
                form.TypeText(m => m.Title, "system test Title");
                form.TypeText(m => m.FirstName, "system test FirstName");
                form.TypeText(m => m.OtherNames, "system test OtherNames");
                form.TypeText(m => m.SurnameOrFamilyName, "system test FamilyName");
                form.TypeDate(m => m.DateOfBirth, dob);
                form.BlurDate(m => m.DateOfBirth);
                form.SelectRadio(m => m.PreviouslyLookedAfter, true);
                form.SelectRadio(m => m.FullTimeEducation, true);
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

            var expectancyDate = DateTime.UtcNow.Date.AddDays(100);

            {
                var form = App.FormForModel<ExpectedChildren>();
                form.TypeDate(m => m.ExpectancyDate, expectancyDate);
                form.TypeText(m => m.ExpectedBabyCount, "3");
            }

            App.Submit();

            {
                App.ClickButton(BsgButtons.AddChild);
                App.VerifyCanSeeText("Child 1");
                App.ClickButton(BsgButtons.AddChild);
                App.VerifyCanSeeText("Child 2");

                var form = App.FormForModel<ExistingChildren>();

                form.TypeText(m => m.Children[0].FirstName, "c1 first name");
                form.TypeText(m => m.Children[0].Surname, "c1 last name");
                form.TypeDate(m => m.Children[0].DateOfBirth, "01", "02", "2003");
                form.TypeText(m => m.Children[0].RelationshipToChild, "c1 relationship");
                form.SelectRadio(m => m.Children[0].ChildBenefit, true);
                form.SelectRadio(m => m.Children[0].FormalKinshipCare, false);

                form.TypeText(m => m.Children[1].FirstName, "c2 first name");
                form.TypeText(m => m.Children[1].Surname, "c2 last name");
                form.TypeDate(m => m.Children[1].DateOfBirth, "02", "03", "2004");
                form.TypeText(m => m.Children[1].RelationshipToChild, "c2 relationship");
                form.SelectRadio(m => m.Children[1].FormalKinshipCare, true);
            }

            App.ClickButton("");

            {
                var form = App.FormForModel<ApplicantBenefits>();

                form.SelectRadio(m => m.HasExistingBenefit, false);
            }

            App.Submit();

            {
                var form = App.FormForModel<ApplicantBenefits>();

                form.SelectRadio(m => m.ReceivingBenefitForUnder20, true);
                form.SelectRadio(m => m.YouOrPartnerInvolvedInTradeDispute, false);
            }

            App.Submit();

            {
                var form = App.FormForModel<HealthProfessional>();

                form.TypeText(m => m.Pin, "XYZ54321");
            }

            App.Submit();

            {
                var form = App.FormForModel<PaymentDetails>();

                form.SelectRadio(m => m.LackingBankAccount, false);
                form.TypeText(m => m.NameOfAccountHolder, "system test account holder");
                form.TypeText(m => m.NameOfBank, "system test bank name");
                form.TypeText(m => m.SortCode, "01-02-03");
                form.TypeText(m => m.AccountNumber, "01234567");
                form.TypeText(m => m.RollNumber, "roll_number");
            }

            App.Submit();

            {
                var form = App.FormForModel<Declaration>();

                form.Check(m => m.AgreedToLegalStatement, true);
            }

            App.Submit();

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                doc.ApplicantDetails.Title.Should().Be("system test Title");
                doc.ApplicantDetails.FirstName.Should().Be("system test FirstName");
                doc.ApplicantDetails.OtherNames.Should().Be("system test OtherNames");
                doc.ApplicantDetails.SurnameOrFamilyName.Should().Be("system test FamilyName");
                doc.ApplicantDetails.DateOfBirth.Should().Be(dob);
                doc.ApplicantDetails.PreviouslyLookedAfter.Should().BeTrue();
                doc.ApplicantDetails.FullTimeEducation.Should().BeTrue();
                doc.ApplicantDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
                doc.ApplicantDetails.CurrentAddress.Street1.Should().Be("system test ca.Street1");
                doc.ApplicantDetails.CurrentAddress.Street2.Should().Be("system test ca.Street2");
                doc.ApplicantDetails.CurrentAddress.TownOrCity.Should().Be("system test ca.TownOrCity");
                doc.ApplicantDetails.CurrentAddress.Postcode.Should().Be("system test ca.Postcode");
                doc.ApplicantDetails.CurrentAddress.DateMovedIn.Should().Be(new DateTime(2004, 03, 02));
                doc.ApplicantDetails.CurrentAddressStatus.Should().Be(AddressStatus.Permanent);
                doc.ApplicantDetails.ContactPreference.Should().Be( ContactPreference.Email);
                doc.ApplicantDetails.EmailAddress.Should().Be("test.system@system.test");

                doc.ExpectedChildren.ExpectancyDate.Should().Be(expectancyDate);
                doc.ExpectedChildren.ExpectedBabyCount.Should().Be(3);

                doc.ExistingChildren.Children.Count.Should().Be(2);

                doc.ExistingChildren.Children[0].FirstName.Should().Be("c1 first name");
                doc.ExistingChildren.Children[0].Surname.Should().Be("c1 last name");
                doc.ExistingChildren.Children[0].DateOfBirth.Should().Be(new DateTime(2003, 02, 01));
                doc.ExistingChildren.Children[0].RelationshipToChild.Should().Be("c1 relationship");
                doc.ExistingChildren.Children[0].ChildBenefit.Should().BeTrue();
                doc.ExistingChildren.Children[0].FormalKinshipCare.Should().BeFalse();

                doc.ExistingChildren.Children[1].FirstName.Should().Be("c2 first name");
                doc.ExistingChildren.Children[1].Surname.Should().Be("c2 last name");
                doc.ExistingChildren.Children[1].DateOfBirth.Should().Be(new DateTime(2004, 03, 02));
                doc.ExistingChildren.Children[1].RelationshipToChild.Should().Be("c2 relationship");
                doc.ExistingChildren.Children[1].ChildBenefit.Should().BeNull();
                doc.ExistingChildren.Children[1].FormalKinshipCare.Should().BeTrue();

                doc.ApplicantBenefits.HasExistingBenefit.Should().BeFalse();
                doc.ApplicantBenefits.ReceivingBenefitForUnder20.Should().BeTrue();
                doc.ApplicantBenefits.YouOrPartnerInvolvedInTradeDispute.Should().BeFalse();

                doc.HealthProfessional.Pin.Should().Be("XYZ54321");

                doc.PaymentDetails.LackingBankAccount.Should().BeFalse();
                doc.PaymentDetails.NameOfAccountHolder.Should().Be("system test account holder");
                doc.PaymentDetails.NameOfBank.Should().Be("system test bank name");
                doc.PaymentDetails.SortCode.Should().Be("01-02-03");
                doc.PaymentDetails.AccountNumber.Should().Be("01234567");
                doc.PaymentDetails.RollNumber.Should().Be("roll_number");
            });
        }
    }
}
