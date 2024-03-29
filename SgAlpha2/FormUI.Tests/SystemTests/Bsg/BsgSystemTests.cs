﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.Util;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;
using OpenQA.Selenium;

namespace FormUI.Tests.SystemTests.Bsg
{
    [TestFixture]
    public class BsgSystemTests : SystemTest
    {
        private bool            _runningAlltests;
        private IList<Sections> _verifiedSections = new List<Sections>();

        [TestFixtureTearDown]
        protected void TestFixtureTearDown()
        {
            base.TearDown();

            if (TestRegistry.TestHasFailed)
                return;

            if (!_runningAlltests)
                return;

            // verify each section has been tested
            foreach (Sections section in Enum.GetValues(typeof(Sections)))
                if (!FeatureToggles.SkipWorkInProgressSection(section))
                    _verifiedSections.Should().Contain(section, "section {0} should be filled in and verified", section);
        }

        [Test]
        public void RunningAllTests()
        {
            // this is to verify in the TestFixtureTearDown that we've verified all sections of the form
            _runningAlltests = true;
        }

        [Test]
        public void CompleteApplication()
        {
            App.GoTo(BsgActions.BeforeYouApply());
            App.VerifyCanSeeText("Before You Apply");
            App.Submit();

            FillInConsent();
            App.Submit();

            App.VerifyCanSeeText("confirm who you are");
            App.Submit();

            var dob = DateTime.Now.Date.AddYears(-19);
            FillInApplicantDetails(dob, previouslyLookedAfter: false, fullTimeEducation: false);
            App.Submit();

            var expectancyDate = DateTime.UtcNow.Date.AddDays(100);
            FillInExpectedChildren(expectancyDate);
            App.Submit();

            FillInExistingChildren();
            App.ClickButton("");

            FillInApplicantBenefits();
            App.Submit();

            App.VerifyCanSeeText("partner's benefits");
            FillInPartnerBenefits();
            App.Submit();

            var partnerDob = DateTime.Now.Date.AddYears(-21);
            FillInPartnerDetails(partnerDob);
            App.Submit();

            FillInHealthProfessional();
            App.Submit();

            FillInPaymentDetails();
            App.Submit();

            var filename = FillInEvidence();
            App.ClickButton("");

            FillInDeclaration();
            App.Submit();

            App.VerifyCanSeeText("Thank you");

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                VerifyConsent(doc);
                VerifyUKVerify(doc);
                VerifyApplicantDetails(doc, dob, false, false);
                VerifyExpectedChildren(doc, expectancyDate);
                VerifyExistingChildren(doc);
                VerifyApplicantBenefits(doc);
                VerifyPartnerBenefits(doc);
                VerifyPartnerDetails(doc, partnerDob);
                VerifyHealthProfessional(doc);
                VerifyPaymentDetails(doc);
                VerifyEvidence(doc, filename);
                VerifyDeclaration(doc);
            });
        }

        [Test]
        public void IncludingGuardianDetails()
        {
            App.GoTo(BsgActions.BeforeYouApply());
            App.VerifyCanSeeText("Before You Apply");
            App.Submit();

            FillInConsent();
            App.Submit();

            App.VerifyCanSeeText("confirm who you are");
            App.Submit();

            var dob = DateTime.Now.Date.AddYears(-18);
            FillInApplicantDetails(dob, previouslyLookedAfter: false, fullTimeEducation: true);
            App.Submit();

            var expectancyDate = DateTime.UtcNow.Date.AddDays(100);
            FillInExpectedChildren(expectancyDate);
            App.Submit();

            App.VerifyCanSeeText("in the household");
            FillInNoExistingChildren();
            App.ClickButton("");

            App.VerifyCanSeeText("guardian's benefits");
            FillInGuardianBenefits();
            App.Submit();

            App.VerifyCanSeeText("guardian's partner's benefits");
            FillInGuardianPartnerBenefits();
            App.Submit();

            var guardianDob = DateTime.Now.Date.AddYears(-39);
            FillInGuardianDetails(guardianDob);
            App.Submit();

            var guardianPartnerDob = DateTime.Now.Date.AddYears(-38);
            FillInGuardianPartnerDetails(guardianPartnerDob);
            App.Submit();

            Db(r =>
            {
                var doc = r.Query<BestStartGrant>().ToList().Single();

                VerifyGuardianBenefits(doc);
                VerifyGuardianPartnerBenefits(doc);
                VerifyGuardianDetails(doc, guardianDob);
                VerifyGuardianPartnerDetails(doc, guardianPartnerDob);
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

        private void FillInApplicantDetails(DateTime dob, bool? previouslyLookedAfter, bool? fullTimeEducation)
        {
            var form = App.FormForModel<ApplicantDetails>();
            form.TypeText(m => m.Title, "system test Title");
            form.TypeText(m => m.FirstName, "system test FirstName");
            form.TypeText(m => m.OtherNames, "system test OtherNames");
            form.TypeText(m => m.SurnameOrFamilyName, "system test FamilyName");
            form.TypeDate(m => m.DateOfBirth, dob);
            form.BlurDate(m => m.DateOfBirth);

            if (previouslyLookedAfter.HasValue)
                form.SelectRadio(m => m.PreviouslyLookedAfter, previouslyLookedAfter.Value);

            if (fullTimeEducation.HasValue)
                form.SelectRadio(m => m.FullTimeEducation, fullTimeEducation.Value);

            form.TypeText(m => m.NationalInsuranceNumber, "AB123456C");
            form.TypeText(m => m.CurrentAddress.Line1, "system test ca.line1");
            form.TypeText(m => m.CurrentAddress.Line2, "system test ca.line2");
            form.TypeText(m => m.CurrentAddress.Line3, "system test ca.line3");
            form.TypeText(m => m.CurrentAddress.Postcode, "system test ca.Postcode");
            form.TypeText(m => m.EmailAddress, "test.system@system.test");
        }

        private void VerifyApplicantDetails(BestStartGrant doc, DateTime dob, bool? previouslyLookedAfter, bool? fullTimeEducation)
        {
            doc.ApplicantDetails.Title.Should().Be("system test Title");
            doc.ApplicantDetails.FirstName.Should().Be("system test FirstName");
            doc.ApplicantDetails.OtherNames.Should().Be("system test OtherNames");
            doc.ApplicantDetails.SurnameOrFamilyName.Should().Be("system test FamilyName");
            doc.ApplicantDetails.DateOfBirth.Should().Be(dob);
            doc.ApplicantDetails.PreviouslyLookedAfter.Should().Be(previouslyLookedAfter);
            doc.ApplicantDetails.FullTimeEducation.Should().Be(fullTimeEducation);
            doc.ApplicantDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
            doc.ApplicantDetails.CurrentAddress.Line1.Should().Be("system test ca.line1");
            doc.ApplicantDetails.CurrentAddress.Line2.Should().Be("system test ca.line2");
            doc.ApplicantDetails.CurrentAddress.Line3.Should().Be("system test ca.line3");
            doc.ApplicantDetails.CurrentAddress.Postcode.Should().Be("system test ca.Postcode");
            doc.ApplicantDetails.EmailAddress.Should().Be("test.system@system.test");
            _verifiedSections.Add(Sections.ApplicantDetails);
        }

        private void FillInExpectedChildren(DateTime expectancyDate)
        {
            var form = App.FormForModel<ExpectedChildren>();
            form.SelectRadio(m => m.IsBabyExpected, true);
            form.TypeDate(m => m.ExpectancyDate, expectancyDate);
            form.SelectRadio(m => m.IsMoreThan1BabyExpected, true);
            form.TypeText(m => m.ExpectedBabyCount, "3");
        }

        private void VerifyExpectedChildren(BestStartGrant doc, DateTime expectancyDate)
        {
            doc.ExpectedChildren.ExpectancyDate.Should().Be(expectancyDate);
            doc.ExpectedChildren.ExpectedBabyCount.Should().Be(3);
            _verifiedSections.Add(Sections.ExpectedChildren);
        }

        private void FillInNoExistingChildren()
        {
            var form = App.FormForModel<ExistingChildren>();
            form.SelectRadio(m => m.AnyExistingChildren, false);
        }

        private void FillInExistingChildren()
        {
            var form = App.FormForModel<ExistingChildren>();

            form.SelectRadio(m => m.AnyExistingChildren, true);

            App.VerifyCanSeeText("Child 1");
            App.ClickButton(BsgButtons.AddChild);
            App.VerifyCanSeeText("Child 2");

            form.TypeText(m => m.Children[0].FirstName, "c1 first name");
            form.TypeText(m => m.Children[0].Surname, "c1 last name");
            form.TypeDate(m => m.Children[0].DateOfBirth, "01", "02", "2003");
            form.SelectRadio(m => m.Children[0].Relationship, Relationship.Parent);
            form.SelectRadio(m => m.Children[0].ChildBenefit, true);

            form.TypeText(m => m.Children[1].FirstName, "c2 first name");
            form.TypeText(m => m.Children[1].Surname, "c2 last name");
            form.TypeDate(m => m.Children[1].DateOfBirth, "02", "03", "2004");
            form.SelectRadio(m => m.Children[1].Relationship, Relationship.KinshipCarer);
            form.SelectRadio(m => m.Children[1].ChildBenefit, false);
            form.TypeTextArea(m => m.Children[1].NoChildBenefitReason, "reason text");
        }

        private void VerifyExistingChildren(BestStartGrant doc)
        {
            doc.ExistingChildren.AnyExistingChildren.Should().BeTrue();
            doc.ExistingChildren.Children.Count.Should().Be(2);

            doc.ExistingChildren.Children[0].FirstName.Should().Be("c1 first name");
            doc.ExistingChildren.Children[0].Surname.Should().Be("c1 last name");
            doc.ExistingChildren.Children[0].DateOfBirth.Should().Be(new DateTime(2003, 02, 01));
            doc.ExistingChildren.Children[0].Relationship.Should().Be(Relationship.Parent);
            doc.ExistingChildren.Children[0].ChildBenefit.Should().BeTrue();

            doc.ExistingChildren.Children[1].FirstName.Should().Be("c2 first name");
            doc.ExistingChildren.Children[1].Surname.Should().Be("c2 last name");
            doc.ExistingChildren.Children[1].DateOfBirth.Should().Be(new DateTime(2004, 03, 02));
            doc.ExistingChildren.Children[1].Relationship.Should().Be(Relationship.KinshipCarer);
            doc.ExistingChildren.Children[1].ChildBenefit.Should().BeFalse();
            doc.ExistingChildren.Children[1].NoChildBenefitReason.Should().Be("reason text");
            _verifiedSections.Add(Sections.ExistingChildren);
        }

        private void FillInApplicantBenefits()
        {
            var form = App.FormForModel<Benefits>();

            form.Check(m => m.Unknown, true);
        }

        private void VerifyApplicantBenefits(BestStartGrant doc)
        {
            doc.ApplicantBenefits.Unknown.Should().BeTrue();
            _verifiedSections.Add(Sections.ApplicantBenefits);
        }

        private void FillInPartnerBenefits()
        {
            var form = App.FormForModel<Benefits>();

            form.Check(m => m.HasIncomeSupport, true);
            form.Check(m => m.HasIncomeBasedJobseekersAllowance, true);
            form.Check(m => m.HasIncomeRelatedEmplymentAndSupportAllowance, true);
            form.Check(m => m.HasUniversalCredit, true);
            form.Check(m => m.HasChildTaxCredit, true);
            form.Check(m => m.HasWorkingTextCredit, true);
            form.Check(m => m.HasHousingBenefit, true);
            form.Check(m => m.HasPensionCredit, true);
        }

        private void VerifyPartnerBenefits(BestStartGrant doc)
        {
            doc.PartnerBenefits.HasIncomeSupport.Should().BeTrue();
            doc.PartnerBenefits.HasIncomeBasedJobseekersAllowance.Should().BeTrue();
            doc.PartnerBenefits.HasIncomeRelatedEmplymentAndSupportAllowance.Should().BeTrue();
            doc.PartnerBenefits.HasUniversalCredit.Should().BeTrue();
            doc.PartnerBenefits.HasChildTaxCredit.Should().BeTrue();
            doc.PartnerBenefits.HasWorkingTextCredit.Should().BeTrue();
            doc.PartnerBenefits.HasHousingBenefit.Should().BeTrue();
            doc.PartnerBenefits.HasPensionCredit.Should().BeTrue();
            _verifiedSections.Add(Sections.PartnerBenefits);
        }

        private void FillInGuardianBenefits()
        {
            var form = App.FormForModel<Benefits>();

            form.Check(m => m.Unknown, true);
        }

        private void VerifyGuardianBenefits(BestStartGrant doc)
        {
            doc.GuardianBenefits.Unknown.Should().BeTrue();
            _verifiedSections.Add(Sections.GuardianBenefits);
        }

        private void FillInGuardianPartnerBenefits()
        {
            var form = App.FormForModel<Benefits>();

            form.Check(m => m.HasIncomeSupport, true);
        }

        private void VerifyGuardianPartnerBenefits(BestStartGrant doc)
        {
            doc.GuardianPartnerBenefits.HasIncomeSupport.Should().BeTrue();
            _verifiedSections.Add(Sections.GuardianPartnerBenefits);
        }

        private void FillInPartnerDetails(DateTime partnerDob)
        {
            var form = App.FormForModel<RelationDetails>();

            form.TypeText(m => m.Title, "p.title");
            form.TypeText(m => m.FullName, "p.fullname");
            form.TypeDate(m => m.DateOfBirth, partnerDob);
            form.TypeText(m => m.NationalInsuranceNumber, "EF234567G");

            form.TypeText(m => m.Address.Line1, "p.line1");
            form.TypeText(m => m.Address.Line2, "p.line2");
            form.TypeText(m => m.Address.Line3, "p.line3");
            form.TypeText(m => m.Address.Postcode, "p.postcode");
        }

        private void VerifyPartnerDetails(BestStartGrant doc, DateTime partnerDob)
        {
            doc.PartnerDetails.Title.Should().Be("p.title");
            doc.PartnerDetails.FullName.Should().Be("p.fullname");
            doc.PartnerDetails.DateOfBirth.Should().Be(partnerDob);
            doc.PartnerDetails.NationalInsuranceNumber.Should().Be("EF 23 45 67 G");

            doc.PartnerDetails.Address.Line1.Should().Be("p.line1");
            doc.PartnerDetails.Address.Line2.Should().Be("p.line2");
            doc.PartnerDetails.Address.Line3.Should().Be("p.line3");
            doc.PartnerDetails.Address.Postcode.Should().Be("p.postcode");

            _verifiedSections.Add(Sections.PartnerDetails);
        }

        private void FillInGuardianDetails(DateTime guardianDob)
        {
            var form = App.FormForModel<RelationDetails>();

            form.TypeText(m => m.Title, "g.title");
            form.TypeText(m => m.FullName, "g.fullname");
            form.TypeDate(m => m.DateOfBirth, guardianDob);
            form.TypeText(m => m.NationalInsuranceNumber, "BC234567D");
            form.TypeText(m => m.RelationshipToApplicant, "ga.parent");

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

            doc.GuardianDetails.Address.Line1.Should().Be("ga.line1");
            doc.GuardianDetails.Address.Line2.Should().Be("ga.line2");
            doc.GuardianDetails.Address.Line3.Should().Be("ga.line3");
            doc.GuardianDetails.Address.Postcode.Should().Be("ga.postcode");
            _verifiedSections.Add(Sections.GuardianDetails);
        }

        private void FillInGuardianPartnerDetails(DateTime guardianPartnerDob)
        {
            var form = App.FormForModel<RelationDetails>();

            form.TypeText(m => m.Title, "gp.title");
            form.TypeText(m => m.FullName, "gp.fullname");
            form.TypeDate(m => m.DateOfBirth, guardianPartnerDob);
            form.TypeText(m => m.NationalInsuranceNumber, "CD234567E");
            form.TypeText(m => m.RelationshipToApplicant, "gp.parent");

            form.TypeText(m => m.Address.Line1, "gp.line1");
            form.TypeText(m => m.Address.Line2, "gp.line2");
            form.TypeText(m => m.Address.Line3, "gp.line3");
            form.TypeText(m => m.Address.Postcode, "gp.postcode");
        }

        private void VerifyGuardianPartnerDetails(BestStartGrant doc, DateTime guardianPartnerDob)
        {
            doc.GuardianPartnerDetails.Title.Should().Be("gp.title");
            doc.GuardianPartnerDetails.FullName.Should().Be("gp.fullname");
            doc.GuardianPartnerDetails.DateOfBirth.Should().Be(guardianPartnerDob);
            doc.GuardianPartnerDetails.NationalInsuranceNumber.Should().Be("CD 23 45 67 E");
            doc.GuardianPartnerDetails.RelationshipToApplicant.Should().Be("gp.parent");

            doc.GuardianPartnerDetails.Address.Line1.Should().Be("gp.line1");
            doc.GuardianPartnerDetails.Address.Line2.Should().Be("gp.line2");
            doc.GuardianPartnerDetails.Address.Line3.Should().Be("gp.line3");
            doc.GuardianPartnerDetails.Address.Postcode.Should().Be("gp.postcode");
            _verifiedSections.Add(Sections.GuardianPartnerDetails);
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

            form.SelectRadio(m => m.HasBankAccount, true);
            form.TypeText(m => m.NameOfAccountHolder, "system test account holder");
            form.TypeText(m => m.NameOfBank, "system test bank name");
            form.TypeText(m => m.SortCode, "01-02-03");
            form.TypeText(m => m.AccountNumber, "01234567");
            form.TypeText(m => m.RollNumber, "roll_number");
        }

        private void VerifyPaymentDetails(BestStartGrant doc)
        {
            doc.PaymentDetails.HasBankAccount.Should().BeTrue();
            doc.PaymentDetails.NameOfAccountHolder.Should().Be("system test account holder");
            doc.PaymentDetails.NameOfBank.Should().Be("system test bank name");
            doc.PaymentDetails.SortCode.Should().Be("01-02-03");
            doc.PaymentDetails.AccountNumber.Should().Be("01234567");
            doc.PaymentDetails.RollNumber.Should().Be("roll_number");
            _verifiedSections.Add(Sections.PaymentDetails);
        }

        private string FillInEvidence()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "some test content");
            App.FindElement($"select file {tempFile}", By.Name("file"), e => e.SendKeys(tempFile));
            App.ClickButton(BsgButtons.UploadFile);

            var form = App.FormForModel<Evidence>();
            form.Check(m => m.SendingByPost, true);

            File.Delete(tempFile);
            return Path.GetFileName(tempFile);
        }

        private void VerifyEvidence(BestStartGrant doc, string filename)
        {
            doc.Evidence.Files.Count.Should().Be(1);
            doc.Evidence.Files[0].Name.Should().Be(filename);

            var cloudStore = DomainRegistry.CloudStore as LocalCloudStore;
            var storedContent = cloudStore.Retrieve("bsg-" + doc.Id, doc.Evidence.Files[0].CloudName);
            Encoding.ASCII.GetString(storedContent).Should().Be("some test content");

            doc.Evidence.SendingByPost.Should().BeTrue();
            _verifiedSections.Add(Sections.Evidence);
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

        private void VerifyUKVerify(BestStartGrant doc)
        {
            _verifiedSections.Add(Sections.UKVerify);
        }
    }
}
