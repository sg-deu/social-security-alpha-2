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
        public void AddApplicantDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m => { });
            ApplicantDetailsShouldBeValid(form, m => m.Title = null);
            ApplicantDetailsShouldBeValid(form, m => m.OtherNames = null);
            ApplicantDetailsShouldBeValid(form, m => m.CurrentAddress.Street2 = null);
            ApplicantDetailsShouldBeValid(form, m => m.DateOfBirth = TestNowUtc - TimeSpan.FromDays(1));

            ApplicantDetailsShouldBeInvalid(form, m => m.FirstName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.SurnameOrFamilyName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = TestNowUtc);
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Street1 = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.TownOrCity = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Postcode = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.DateMovedIn = null);
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
            ExpectedChildrenShouldBeValid(form, m => m.ExpectancyDate = TestNowUtc);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 1);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 10);

            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectancyDate = TestNowUtc - TimeSpan.FromDays(1));
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
        public void AddApplicantBenefits_Part1DoesNotOverwritePart2()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantBenefits, ApplicantBenefitsBuilder.NewValid(Part.Part2, ab => ab.ReceivingBenefitForUnder20 = true))
                .Insert();

            var part1 = new ApplicantBenefits
            {
                HasExistingBenefit = false,
            };

            form.AddApplicantBenefits(Part.Part1, part1);

            form.ApplicantBenefits.ReceivingBenefitForUnder20.Should().BeTrue();
        }

        [Test]
        public void AddApplicantBenefits_Part2DoesNotOverwritePart1()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var part1 = new ApplicantBenefits
            {
                HasExistingBenefit = false,
            };

            form.AddApplicantBenefits(Part.Part1, part1);

            form.ApplicantBenefits.HasExistingBenefit.Should().BeFalse();

            var part2 = new ApplicantBenefits
            {
                ReceivingBenefitForUnder20 = true,
                YouOrPartnerInvolvedInTradeDispute = false,
            };

            form.AddApplicantBenefits(Part.Part2, part2);

            form.ApplicantBenefits.HasExistingBenefit.Should().BeFalse("part1 should not be lost");
            form.ApplicantBenefits.ReceivingBenefitForUnder20.Should().BeTrue();
            form.ApplicantBenefits.YouOrPartnerInvolvedInTradeDispute.Should().BeFalse();
        }

        [Test]
        public void AddApplicantBenefits_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantBenefitsShouldBeValid(form, Part.Part1, m => { });
            ApplicantBenefitsShouldBeValid(form, Part.Part1, m => m.ReceivingBenefitForUnder20 = null);
            ApplicantBenefitsShouldBeValid(form, Part.Part1, m => m.YouOrPartnerInvolvedInTradeDispute = null);
            ApplicantBenefitsShouldBeValid(form, Part.Part2, m => { });

            ApplicantBenefitsShouldBeInvalid(form, Part.Part1, m => m.HasExistingBenefit = null);
            ApplicantBenefitsShouldBeInvalid(form, Part.Part2, m => m.ReceivingBenefitForUnder20 = null);
            ApplicantBenefitsShouldBeInvalid(form, Part.Part2, m => m.YouOrPartnerInvolvedInTradeDispute = null);
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
            ShouldBeValid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(mutator)));
        }

        protected void ExistingChildrenShouldBeInvalid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(mutator)));
        }

        protected void ApplicantBenefitsShouldBeValid(BestStartGrant form, Part part, Action<ApplicantBenefits> mutator)
        {
            ShouldBeValid(() => form.AddApplicantBenefits(part, ApplicantBenefitsBuilder.NewValid(part, mutator)));
        }

        protected void ApplicantBenefitsShouldBeInvalid(BestStartGrant form, Part part, Action<ApplicantBenefits> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantBenefits(part, ApplicantBenefitsBuilder.NewValid(part, mutator)));
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
