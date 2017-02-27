using System;
using System.Collections.Generic;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrantTests : DomainTest
    {
        [Test]
        public void Start_Validation()
        {
            AboutYouShouldBeValid(m => { });
            AboutYouShouldBeValid(m => m.Title = null);
            AboutYouShouldBeValid(m => m.OtherNames = null);
            AboutYouShouldBeValid(m => m.CurrentAddress.Street2 = null);
            AboutYouShouldBeValid(m => m.DateOfBirth = TestNowUtc - TimeSpan.FromDays(1));

            AboutYouShouldBeInvalid(m => m.FirstName = null);
            AboutYouShouldBeInvalid(m => m.SurnameOrFamilyName = null);
            AboutYouShouldBeInvalid(m => m.DateOfBirth = null);
            AboutYouShouldBeInvalid(m => m.DateOfBirth = TestNowUtc);
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = null);
            AboutYouShouldBeInvalid(m => m.CurrentAddress.Street1 = null);
            AboutYouShouldBeInvalid(m => m.CurrentAddress.TownOrCity = null);
            AboutYouShouldBeInvalid(m => m.CurrentAddress.Postcode = null);
            AboutYouShouldBeInvalid(m => m.CurrentAddress.DateMovedIn = null);
            AboutYouShouldBeInvalid(m => m.CurrentAddressStatus = null);
            AboutYouShouldBeInvalid(m => m.ContactPreference = null);
        }

        [Test]
        public void Start_ContactPreferenceEmail_RequiresEmail()
        {
            AboutYouShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.PhoneNumer = null;
                m.MobilePhoneNumber = null;
            });
            AboutYouShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.EmailAddress = null;
            });
        }

        [Test]
        public void Start_ContactPreferencePhone_RequiresPhoneNumber()
        {
            AboutYouShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.EmailAddress = null;
                m.MobilePhoneNumber = null;
            });
            AboutYouShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.PhoneNumer = null;
            });
        }

        [Test]
        public void Start_ContactPreferenceText_RequiresText()
        {
            AboutYouShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.EmailAddress = null;
                m.PhoneNumer = null;
            });
            AboutYouShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.MobilePhoneNumber = null;
            });
        }

        [Test]
        public void Start_NationalInsuranceNumber_FormattedCorrectly()
        {
            AboutYouShouldBeValid(m => m.NationalInsuranceNumber = "AB 12 34 56 C", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            AboutYouShouldBeValid(m => m.NationalInsuranceNumber = "ab 12 34 56 c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            AboutYouShouldBeValid(m => m.NationalInsuranceNumber = "Ab123456c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            AboutYouShouldBeValid(m => m.NationalInsuranceNumber = "AB/123456/c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));

            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "A 12 34 56 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 34 56 CD");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12/34/56 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "A. 12 34 56 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "AB .2 34 56 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 .4 56 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 34 .6 C");
            AboutYouShouldBeInvalid(m => m.NationalInsuranceNumber = "A5 12 34 56 .");
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

        #region test helpers

        protected void AboutYouShouldBeValid(Action<AboutYou> mutator, Action<AboutYou> postVerify = null)
        {
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            ShouldBeValid(() => BestStartGrant.Start(aboutYou));

            if (postVerify != null)
                postVerify(aboutYou);
        }

        protected void AboutYouShouldBeInvalid(Action<AboutYou> mutator)
        {
            ShouldBeInvalid(() => BestStartGrant.Start(AboutYouBuilder.NewValid(mutator)));
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

        #endregion
    }
}
