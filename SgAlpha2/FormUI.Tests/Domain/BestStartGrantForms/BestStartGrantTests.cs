using System;
using System.Collections.Generic;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrantTests : DomainTest
    {
        [Test]
        public void Consent_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ConsentShouldBeValid(form, m => { });

            ConsentShouldBeInvalid(form, m => m.AgreedToConsent = false);
        }

        [Test]
        public void NextSectionClearsSkippedSections()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid(ad => ad.Under16(TestNowUtc.Value)))
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewValid())
                .With(f => f.Declaration, DeclarationBuilder.NewValid())
                .Insert();

            form.AddExistingChildren(ExistingChildrenBuilder.NewValid());

            form = Repository.Load<BestStartGrant>(form.Id);

            form.Declaration.Should().NotBeNull("should not be overwritten by moving to the next section");
            form.ApplicantBenefits.Should().BeNull("intermediate 'ApplicantBenefits' section should be cleared based on answers");
        }

        [Test]
        public void ApplicantDetails_RequiresCareQuestion()
        {
            TestNowUtc = new DateTime(2009, 08, 07, 06, 05, 04);
            var applicantDetails = ApplicantDetailsBuilder.NewValid();

            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeTrue("default builder should ask question");

            applicantDetails.DateOfBirth = null;
            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeFalse("no need to ask question if DoB not supplied");

            // applicant is 25 today
            applicantDetails.DateOfBirth = new DateTime(1984, 08, 07);
            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeFalse("no need to ask question if applicant >= 25");

            // applicant is 25 tomorrow
            applicantDetails.DateOfBirth = new DateTime(1984, 08, 08);
            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeTrue("ask question if applicant is still 24");

            // applicant is 18 today
            applicantDetails.DateOfBirth = new DateTime(1991, 08, 07);
            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeTrue("ask question if applicant has turned 18");

            // applicant is 18 tomorrow
            applicantDetails.DateOfBirth = new DateTime(1991, 08, 08);
            BestStartGrant.ShouldAskCareQuestion(applicantDetails).Should().BeFalse("no need to ask question if applicant is under 18");
        }

        [Test]
        public void ApplicantDetails_RequiresEducationQuestion()
        {
            TestNowUtc = new DateTime(2009, 08, 07, 06, 05, 04);
            var applicantDetails = ApplicantDetailsBuilder.NewValid();

            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeTrue("default builder should ask question");

            applicantDetails.DateOfBirth = null;
            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeFalse("no need to ask question if DoB not supplied");

            // applicant is 20 today
            applicantDetails.DateOfBirth = new DateTime(1989, 08, 07);
            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeFalse("no need to ask question if applicant >= 20");

            // applicant is 20 tomorrow (still 19)
            applicantDetails.DateOfBirth = new DateTime(1989, 08, 08);
            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeTrue("ask question if applicant is still 19");

            // applicant is 18 today
            applicantDetails.DateOfBirth = new DateTime(1991, 08, 07);
            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeTrue("ask question if applicant has turned 18");

            // applicant is 18 tomorrow
            applicantDetails.DateOfBirth = new DateTime(1991, 08, 08);
            BestStartGrant.ShouldAskEducationQuestion(applicantDetails).Should().BeFalse("no need to ask question if applicant is under 18");
        }

        [Test]
        public void AddApplicantDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m => { });
            ApplicantDetailsShouldBeValid(form, m => m.Title = null);
            ApplicantDetailsShouldBeValid(form, m => m.OtherNames = null);
            ApplicantDetailsShouldBeValid(form, m => m.CurrentAddress.Line3 = null);
            ApplicantDetailsShouldBeValid(form, m => m.DateOfBirth = TestNowUtc - TimeSpan.FromDays(1));
            ApplicantDetailsShouldBeValid(form, m => { m.DateOfBirth = TestNowUtc.Value.AddYears(-30); m.PreviouslyLookedAfter = null; });
            ApplicantDetailsShouldBeValid(form, m => { m.DateOfBirth = TestNowUtc.Value.AddYears(-30); m.FullTimeEducation = null; });

            ApplicantDetailsShouldBeInvalid(form, m => m.FirstName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.SurnameOrFamilyName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = TestNowUtc);
            ApplicantDetailsShouldBeInvalid(form, m => m.PreviouslyLookedAfter = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.FullTimeEducation = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Line1 = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Line2 = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Postcode = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateMovedIn = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddressStatus = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.ContactPreference = null);
        }

        [Test]
        public void AddApplicantDetails_ContactPreferenceEmail_RequiresEmail()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.PhoneNumer = null;
                m.MobilePhoneNumber = null;
            });
            ApplicantDetailsShouldBeInvalid(form, m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.EmailAddress = null;
            });
        }

        [Test]
        public void AddApplicantDetails_ContactPreferencePhone_RequiresPhoneNumber()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.EmailAddress = null;
                m.MobilePhoneNumber = null;
            });
            ApplicantDetailsShouldBeInvalid(form, m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.PhoneNumer = null;
            });
        }

        [Test]
        public void AddApplicantDetails_ContactPreferenceText_RequiresText()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.EmailAddress = null;
                m.PhoneNumer = null;
            });
            ApplicantDetailsShouldBeInvalid(form, m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.MobilePhoneNumber = null;
            });
        }

        [Test]
        public void AddApplicantDetails_NationalInsuranceNumber_FormattedCorrectly()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m => m.NationalInsuranceNumber = "AB 12 34 56 C", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ApplicantDetailsShouldBeValid(form, m => m.NationalInsuranceNumber = "ab 12 34 56 c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ApplicantDetailsShouldBeValid(form, m => m.NationalInsuranceNumber = "Ab123456c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ApplicantDetailsShouldBeValid(form, m => m.NationalInsuranceNumber = "AB/123456/c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));

            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "A 12 34 56 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "AB 12 34 56 CD");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "AB 12/34/56 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "A. 12 34 56 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "AB .2 34 56 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "AB 12 .4 56 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "AB 12 34 .6 C");
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = "A5 12 34 56 .");
        }

        [Test]
        public void AddExpectedChildren_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ExpectedChildrenShouldBeValid(form, m => { });
            ExpectedChildrenShouldBeValid(form, m => { m.ExpectancyDate = null; m.ExpectedBabyCount = null; });
            ExpectedChildrenShouldBeValid(form, m => { m.ExpectancyDate = TestNowUtc; m.ExpectedBabyCount = null; });
            ExpectedChildrenShouldBeValid(form, m => m.ExpectancyDate = TestNowUtc);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 1);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 10);

            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectancyDate = TestNowUtc - TimeSpan.FromDays(1));
            ExpectedChildrenShouldBeInvalid(form, m => { m.ExpectancyDate = null; m.ExpectedBabyCount = 1; });
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 0);
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 11);
        }

        [Test]
        public void AddExistingChildren_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ExistingChildrenShouldBeValid(form, m => { });
            ExistingChildrenShouldBeValid(form, m => m.Children = new List<ExistingChild>());

            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].FirstName = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].Surname = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].DateOfBirth = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].DateOfBirth = TestNowUtc);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].RelationshipToChild = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].FormalKinshipCare = null);
        }

        [Test]
        public void AddApplicantBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantBenefitsShouldBeValid(form, m => { });

            ApplicantBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        [Test]
        public void AddPartnerBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PartnerBenefitsShouldBeValid(form, m => { });

            PartnerBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        [Test]
        public void AddGuardianBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianBenefitsShouldBeValid(form, m => { });

            GuardianBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        [Test]
        public void AddGuardianPartnerBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianPartnerBenefitsShouldBeValid(form, m => { });

            GuardianPartnerBenefitsShouldBeInvalid(form, m => m.HasExistingBenefit = null);
        }

        [Test]
        public void AddGuardianDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianDetailsShouldBeValid(form, Part.Part1, m => { });
            GuardianDetailsShouldBeValid(form, Part.Part1, m => m.Title = null);
            GuardianDetailsShouldBeValid(form, Part.Part1, m => m.Address.Line3 = null);
            GuardianDetailsShouldBeValid(form, Part.Part2, m => { });

            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.FullName = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.DateOfBirth = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.DateOfBirth = TestNowUtc);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.NationalInsuranceNumber = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.RelationshipToApplicant = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line1 = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line2 = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Postcode = null);
        }

        [Test]
        public void AddGuardianDetails_FormatsNationalInsuranceNumber()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var details = GuardianDetailsBuilder.NewValid(Part.Part1, d => d.NationalInsuranceNumber = "AB123456C");
            form.AddGuardianDetails(Part.Part1, details);

            form.GuardianDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
        }

        [Test]
        public void AddHealthProfessional_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            HealthProfessionalShouldBeValid(form, m => { });

            HealthProfessionalShouldBeInvalid(form, m => m.Pin = null);
        }

        [Test]
        public void AddPaymentDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => { });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = false; m.RollNumber = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.NameOfAccountHolder = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.NameOfBank = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.SortCode = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.AccountNumber = null; });

            PaymentDetailsShouldBeInvalid(form, m => m.LackingBankAccount = null);
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfAccountHolder = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfBank = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.SortCode = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.AccountNumber = null; });
        }

        [Test]
        public void AddPaymentDetails_SortCodeValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => m.SortCode = "01-02-03");

            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01 02-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-02.03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "x1-02-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-0x-03");
            PaymentDetailsShouldBeInvalid(form, m => m.SortCode = "01-02-.3");
        }

        [Test]
        public void AddPaymentDetails_AccountNumberValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PaymentDetailsShouldBeValid(form, m => m.AccountNumber = "0");
            PaymentDetailsShouldBeValid(form, m => m.AccountNumber = "01234567");

            PaymentDetailsShouldBeInvalid(form, m => m.AccountNumber = "x");
            PaymentDetailsShouldBeInvalid(form, m => m.AccountNumber = " 1 ");
        }

        [Test]
        public void Complete_DeclarationValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            DeclarationShouldBeValid(form, m => { });

            DeclarationShouldBeInvalid(form, m => m.AgreedToLegalStatement = false);
        }

        #region test helpers

        protected void ConsentShouldBeValid(BestStartGrant form, Action<Consent> mutator)
        {
            ShouldBeValid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }

        protected void ConsentShouldBeInvalid(BestStartGrant form, Action<Consent> mutator)
        {
            ShouldBeInvalid(() => form.AddConsent(ConsentBuilder.NewValid(mutator)));
        }

        protected void ApplicantDetailsShouldBeValid(BestStartGrant form, Action<ApplicantDetails> mutator, Action<ApplicantDetails> postVerify = null)
        {
            var applicantDetails = ApplicantDetailsBuilder.NewValid(mutator);
            ShouldBeValid(() => form.AddApplicantDetails(applicantDetails));

            if (postVerify != null)
                postVerify(applicantDetails);
        }

        protected void ApplicantDetailsShouldBeInvalid(BestStartGrant form, Action<ApplicantDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantDetails(ApplicantDetailsBuilder.NewValid(mutator)));
        }

        protected void ExpectedChildrenShouldBeValid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            ShouldBeValid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }

        protected void ExpectedChildrenShouldBeInvalid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }

        protected void ExistingChildrenShouldBeValid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            ShouldBeValid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(2, mutator)));
        }

        protected void ExistingChildrenShouldBeInvalid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(2, mutator)));
        }

        protected void ApplicantBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddApplicantBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void ApplicantBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void PartnerBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void PartnerBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddGuardianBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianPartnerBenefitsShouldBeValid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeValid(() => form.AddGuardianPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianPartnerBenefitsShouldBeInvalid(BestStartGrant form, Action<Benefits> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianPartnerBenefits(BenefitsBuilder.NewValid(mutator)));
        }

        protected void GuardianDetailsShouldBeValid(BestStartGrant form, Part part, Action<GuardianDetails> mutator)
        {
            ShouldBeValid(() => form.AddGuardianDetails(part, GuardianDetailsBuilder.NewValid(part, mutator)));
        }

        protected void GuardianDetailsShouldBeInvalid(BestStartGrant form, Part part, Action<GuardianDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianDetails(part, GuardianDetailsBuilder.NewValid(part, mutator)));
        }

        protected void HealthProfessionalShouldBeValid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            ShouldBeValid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }

        protected void HealthProfessionalShouldBeInvalid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            ShouldBeInvalid(() => form.AddHealthProfessional(HealthProfessionalBuilder.NewValid(mutator)));
        }

        protected void PaymentDetailsShouldBeValid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            ShouldBeValid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }

        protected void PaymentDetailsShouldBeInvalid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddPaymentDetails(PaymentDetailsBuilder.NewValid(mutator)));
        }

        protected void DeclarationShouldBeValid(BestStartGrant form, Action<Declaration> mutator)
        {
            ShouldBeValid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }

        protected void DeclarationShouldBeInvalid(BestStartGrant form, Action<Declaration> mutator)
        {
            ShouldBeInvalid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }

        #endregion
    }
}
