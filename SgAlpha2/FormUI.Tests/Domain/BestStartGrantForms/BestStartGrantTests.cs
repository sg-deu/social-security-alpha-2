using System;
using System.Collections.Generic;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
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
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.AccountNumber = null; });
            PaymentDetailsShouldBeValid(form, m => { m.LackingBankAccount = true; m.SortCode = null; });

            PaymentDetailsShouldBeInvalid(form, m => m.LackingBankAccount = null);
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfAccountHolder = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.NameOfBank = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.AccountNumber = null; });
            PaymentDetailsShouldBeInvalid(form, m => { m.LackingBankAccount = false; m.SortCode = null; });
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

        #region test helpers

        protected void AboutYouShouldBeValid(Action<AboutYou> mutator, Action<AboutYou> postVerify = null)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => BestStartGrant.Start(aboutYou));

            if (postVerify != null)
                postVerify(aboutYou);
        }

        protected void AboutYouShouldBeInvalid(Action<AboutYou> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => BestStartGrant.Start(aboutYou));
        }

        protected void ExpectedChildrenShouldBeValid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var expectedChildren = ExpectedChildrenBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => form.AddExpectedChildren(expectedChildren));
        }

        protected void ExpectedChildrenShouldBeInvalid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var expectedChildren = ExpectedChildrenBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => form.AddExpectedChildren(expectedChildren));
        }

        protected void ExistingChildrenShouldBeValid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var existingChildren = ExistingChildrenBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => form.AddExistingChildren(existingChildren));
        }

        protected void ExistingChildrenShouldBeInvalid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var existingChildren = ExistingChildrenBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => form.AddExistingChildren(existingChildren));
        }

        protected void HealthProfessionalShouldBeValid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var healthProfessional = HealthProfessionalBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => form.AddHealthProfessional(healthProfessional));
        }

        protected void HealthProfessionalShouldBeInvalid(BestStartGrant form, Action<HealthProfessional> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var healthProfessional = HealthProfessionalBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => form.AddHealthProfessional(healthProfessional));
        }

        protected void PaymentDetailsShouldBeValid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var paymentDetails = PaymentDetailsBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => form.AddPaymentDetails(paymentDetails));
        }

        protected void PaymentDetailsShouldBeInvalid(BestStartGrant form, Action<PaymentDetails> mutator)
        {
            DomainRegistry.ValidationContext = new ValidationContext(true);
            var paymentDetails = PaymentDetailsBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => form.AddPaymentDetails(paymentDetails));
        }

        #endregion
    }
}
