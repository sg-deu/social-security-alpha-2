using System;
using System.Collections.Generic;
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
        private IList<Sections> _verifiedSections = new List<Sections>();

        [TestFixtureTearDown]
        protected void TestFixtureTearDown()
        {
            base.TearDown();

            // verify each section has been tested
            foreach (Sections section in Enum.GetValues(typeof(Sections)))
                _verifiedSections.Should().Contain(section, "section {0} should be filled in and verified", section);
        }

        [Test]
        public void CompleteApplication()
        {
            App.GoTo(BsgActions.Overview());
            App.VerifyCanSeeText("Overview");
            App.Submit();

            FillInConsent();
            App.Submit();

            var dob = DateTime.Now.Date.AddYears(-19);
            FillInApplicantDetails(dob, fillInPreviouslyLookedAfter: true, fillInFullTimeEducation: true);
            App.Submit();

            var expectancyDate = DateTime.UtcNow.Date.AddDays(100);
            FillInExpectedChildren(expectancyDate);
            App.Submit();

            FillInExistingChildren();
            App.ClickButton("");

            FillInApplicantBenefits();
            App.Submit();

            FillInHealthProfessional();
            App.Submit();

            FillInPaymentDetails();
            App.Submit();

            FillInDeclaration();
            App.Submit();

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                VerifyConsent(doc);
                VerifyApplicantDetails(doc, dob);
                VerifyExpectedChildren(doc, expectancyDate);
                VerifyExistingChildren(doc);
                VerifyApplicantBenefits(doc);
                VerifyHealthProfessional(doc);
                VerifyPaymentDetails(doc);
                VerifyDeclaration(doc);
            });
        }

        [Test]
        public void IncludingGuardianDetails()
        {
            App.GoTo(BsgActions.Overview());
            App.VerifyCanSeeText("Overview");
            App.Submit();

            FillInConsent();
            App.Submit();

            var dob = DateTime.Now.Date.AddYears(-15);
            FillInApplicantDetails(dob, fillInPreviouslyLookedAfter: false, fillInFullTimeEducation: false);
            App.Submit();

            var expectancyDate = DateTime.UtcNow.Date.AddDays(100);
            FillInExpectedChildren(expectancyDate);
            App.Submit();

            App.VerifyCanSeeText("already been born");
            App.ClickButton("");

            var guardianDob = DateTime.Now.Date.AddYears(-39);
            FillInGuardianDetails1(guardianDob);
            App.Submit();

            FillInGuardianDetails2();
            App.Submit();

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                VerifyGuardianDetails(doc, guardianDob);
            });
        }

        private void FillInConsent()
        {
            var form = App.FormForModel<Consent>();
            form.Check(m => m.AgreedToConsent, true);
        }

        private void VerifyConsent(BestStartGrant doc)
        {
            doc.Consent.AgreedToConsent.Should().BeTrue();
            _verifiedSections.Add(Sections.Consent);
        }

        private void FillInApplicantDetails(DateTime dob, bool fillInPreviouslyLookedAfter, bool fillInFullTimeEducation)
        {
            var form = App.FormForModel<ApplicantDetails>();
            form.TypeText(m => m.Title, "system test Title");
            form.TypeText(m => m.FirstName, "system test FirstName");
            form.TypeText(m => m.OtherNames, "system test OtherNames");
            form.TypeText(m => m.SurnameOrFamilyName, "system test FamilyName");
            form.TypeDate(m => m.DateOfBirth, dob);
            form.BlurDate(m => m.DateOfBirth);

            if (fillInPreviouslyLookedAfter)
                form.SelectRadio(m => m.PreviouslyLookedAfter, true);

            if (fillInFullTimeEducation)
                form.SelectRadio(m => m.FullTimeEducation, true);

            form.TypeText(m => m.NationalInsuranceNumber, "AB123456C");
            form.TypeText(m => m.CurrentAddress.Line1, "system test ca.line1");
            form.TypeText(m => m.CurrentAddress.Line2, "system test ca.line2");
            form.TypeText(m => m.CurrentAddress.Line3, "system test ca.line3");
            form.TypeText(m => m.CurrentAddress.Postcode, "system test ca.Postcode");
            form.TypeDate(m => m.DateMovedIn, "02", "03", "2004");
            form.SelectRadio(m => m.CurrentAddressStatus, AddressStatus.Permanent);
            form.SelectRadio(m => m.ContactPreference, ContactPreference.Email);
            form.TypeText(m => m.EmailAddress, "test.system@system.test");
        }

        private void VerifyApplicantDetails(BestStartGrant doc, DateTime dob)
        {
            doc.ApplicantDetails.Title.Should().Be("system test Title");
            doc.ApplicantDetails.FirstName.Should().Be("system test FirstName");
            doc.ApplicantDetails.OtherNames.Should().Be("system test OtherNames");
            doc.ApplicantDetails.SurnameOrFamilyName.Should().Be("system test FamilyName");
            doc.ApplicantDetails.DateOfBirth.Should().Be(dob);
            doc.ApplicantDetails.PreviouslyLookedAfter.Should().BeTrue();
            doc.ApplicantDetails.FullTimeEducation.Should().BeTrue();
            doc.ApplicantDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
            doc.ApplicantDetails.CurrentAddress.Line1.Should().Be("system test ca.line1");
            doc.ApplicantDetails.CurrentAddress.Line2.Should().Be("system test ca.line2");
            doc.ApplicantDetails.CurrentAddress.Line3.Should().Be("system test ca.line3");
            doc.ApplicantDetails.CurrentAddress.Postcode.Should().Be("system test ca.Postcode");
            doc.ApplicantDetails.DateMovedIn.Should().Be(new DateTime(2004, 03, 02));
            doc.ApplicantDetails.CurrentAddressStatus.Should().Be(AddressStatus.Permanent);
            doc.ApplicantDetails.ContactPreference.Should().Be(ContactPreference.Email);
            doc.ApplicantDetails.EmailAddress.Should().Be("test.system@system.test");
            _verifiedSections.Add(Sections.ApplicantDetails);
        }

        private void FillInExpectedChildren(DateTime expectancyDate)
        {
            var form = App.FormForModel<ExpectedChildren>();
            form.TypeDate(m => m.ExpectancyDate, expectancyDate);
            form.TypeText(m => m.ExpectedBabyCount, "3");
        }

        private void VerifyExpectedChildren(BestStartGrant doc, DateTime expectancyDate)
        {
            doc.ExpectedChildren.ExpectancyDate.Should().Be(expectancyDate);
            doc.ExpectedChildren.ExpectedBabyCount.Should().Be(3);
            _verifiedSections.Add(Sections.ExpectedChildren);
        }

        private void FillInExistingChildren()
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

        private void VerifyExistingChildren(BestStartGrant doc)
        {
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
            _verifiedSections.Add(Sections.ExistingChildren);
        }

        private void FillInApplicantBenefits()
        {
            var form = App.FormForModel<Benefits>();

            form.SelectRadio(m => m.HasExistingBenefit, false);
        }

        private void VerifyApplicantBenefits(BestStartGrant doc)
        {
            doc.ApplicantBenefits.HasExistingBenefit.Should().BeFalse();
            _verifiedSections.Add(Sections.ApplicantBenefits);
        }

        private void FillInGuardianDetails1(DateTime guardianDob)
        {
            var form = App.FormForModel<GuardianDetails>();

            form.TypeText(m => m.Title, "g.title");
            form.TypeText(m => m.FullName, "g.fullname");
            form.TypeDate(m => m.DateOfBirth, guardianDob);
            form.TypeText(m => m.NationalInsuranceNumber, "BC234567D");
            form.TypeText(m => m.RelationshipToApplicant, "ga.parent");
        }

        private void FillInGuardianDetails2()
        {
            var form = App.FormForModel<GuardianDetails>();

            form.TypeText(m => m.Address.Line1, "ga.line1");
            form.TypeText(m => m.Address.Line2, "ga.line2");
            form.TypeText(m => m.Address.Line3, "ga.line3");
            form.TypeText(m => m.Address.Postcode, "ga.postcode");
        }

        private void VerifyGuardianDetails(BestStartGrant doc, DateTime guardianDob)
        {
            doc.GuardianDetails.Title.Should().Be("g.title");
            doc.GuardianDetails.FullName.Should().Be("g.fullname");
            doc.GuardianDetails.DateOfBirth.Should().Be(guardianDob);
            doc.GuardianDetails.NationalInsuranceNumber.Should().Be("BC 23 45 67 D");
            doc.GuardianDetails.RelationshipToApplicant.Should().Be("ga.parent");
            _verifiedSections.Add(Sections.GuardianDetails1);

            doc.GuardianDetails.Address.Line1.Should().Be("ga.line1");
            doc.GuardianDetails.Address.Line2.Should().Be("ga.line2");
            doc.GuardianDetails.Address.Line3.Should().Be("ga.line3");
            doc.GuardianDetails.Address.Postcode.Should().Be("ga.postcode");
            _verifiedSections.Add(Sections.GuardianDetails2);
        }

        private void FillInHealthProfessional()
        {
            var form = App.FormForModel<HealthProfessional>();

            form.TypeText(m => m.Pin, "XYZ54321");
        }

        private void VerifyHealthProfessional(BestStartGrant doc)
        {
            doc.HealthProfessional.Pin.Should().Be("XYZ54321");
            _verifiedSections.Add(Sections.HealthProfessional);
        }

        private void FillInPaymentDetails()
        {
            var form = App.FormForModel<PaymentDetails>();

            form.SelectRadio(m => m.LackingBankAccount, false);
            form.TypeText(m => m.NameOfAccountHolder, "system test account holder");
            form.TypeText(m => m.NameOfBank, "system test bank name");
            form.TypeText(m => m.SortCode, "01-02-03");
            form.TypeText(m => m.AccountNumber, "01234567");
            form.TypeText(m => m.RollNumber, "roll_number");
        }

        private void VerifyPaymentDetails(BestStartGrant doc)
        {
            doc.PaymentDetails.LackingBankAccount.Should().BeFalse();
            doc.PaymentDetails.NameOfAccountHolder.Should().Be("system test account holder");
            doc.PaymentDetails.NameOfBank.Should().Be("system test bank name");
            doc.PaymentDetails.SortCode.Should().Be("01-02-03");
            doc.PaymentDetails.AccountNumber.Should().Be("01234567");
            doc.PaymentDetails.RollNumber.Should().Be("roll_number");
            _verifiedSections.Add(Sections.PaymentDetails);
        }

        private void FillInDeclaration()
        {
            var form = App.FormForModel<Declaration>();

            form.Check(m => m.AgreedToLegalStatement, true);
        }

        private void VerifyDeclaration(BestStartGrant doc)
        {
            doc.Declaration.AgreedToLegalStatement.Should().BeTrue();
            _verifiedSections.Add(Sections.Declaration);
        }
    }
}
