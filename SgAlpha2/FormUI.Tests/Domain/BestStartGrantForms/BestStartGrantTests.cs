using System;
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

            AboutYouShouldBeInvalid(m => m.FirstName = null);
            AboutYouShouldBeInvalid(m => m.SurnameOrFamilyName = null);
            AboutYouShouldBeInvalid(m => m.DateOfBirth = null);
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

            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 1);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 10);

            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 0);
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 11);
        }

        #region test helpers

        protected void AboutYouShouldBeValid(Action<AboutYou> mutator, Action<AboutYou> postVerify = null)
        {
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => BestStartGrant.Start(aboutYou), mutator.ToString());

            if (postVerify != null)
                postVerify(aboutYou);
        }

        protected void AboutYouShouldBeInvalid(Action<AboutYou> mutator)
        {
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => BestStartGrant.Start(aboutYou), mutator.ToString());
        }

        protected void ExpectedChildrenShouldBeValid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            var expectedChildren = ExpectedChildrenBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => form.AddExpectedChildren(expectedChildren), mutator.ToString());
        }

        protected void ExpectedChildrenShouldBeInvalid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            var expectedChildren = ExpectedChildrenBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => form.AddExpectedChildren(expectedChildren), mutator.ToString());
        }

        #endregion
    }
}
