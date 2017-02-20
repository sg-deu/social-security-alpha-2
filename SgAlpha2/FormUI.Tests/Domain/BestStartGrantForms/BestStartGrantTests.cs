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
            ShouldBeValid(m => { });
            ShouldBeValid(m => m.Title = null);
            ShouldBeValid(m => m.OtherNames = null);
            ShouldBeValid(m => m.CurrentAddress.Street2 = null);

            ShouldBeInvalid(m => m.FirstName = null);
            ShouldBeInvalid(m => m.SurnameOrFamilyName = null);
            ShouldBeInvalid(m => m.DateOfBirth = null);
            ShouldBeInvalid(m => m.NationalInsuranceNumber = null);
            ShouldBeInvalid(m => m.CurrentAddress.Street1 = null);
            ShouldBeInvalid(m => m.CurrentAddress.TownOrCity = null);
            ShouldBeInvalid(m => m.CurrentAddress.Postcode = null);
            ShouldBeInvalid(m => m.CurrentAddress.DateMovedIn = null);
            ShouldBeInvalid(m => m.CurrentAddressStatus = null);
            ShouldBeInvalid(m => m.ContactPreference = null);
        }

        [Test]
        public void Start_ContactPreferenceEmail_RequiresEmail()
        {
            ShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.PhoneNumer = null;
                m.MobilePhoneNumber = null;
            });
            ShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Email;
                m.EmailAddress = null;
            });
        }

        [Test]
        public void Start_ContactPreferencePhone_RequiresPhoneNumber()
        {
            ShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.EmailAddress = null;
                m.MobilePhoneNumber = null;
            });
            ShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Phone;
                m.PhoneNumer = null;
            });
        }

        [Test]
        public void Start_ContactPreferenceText_RequiresText()
        {
            ShouldBeValid(m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.EmailAddress = null;
                m.PhoneNumer = null;
            });
            ShouldBeInvalid(m =>
            {
                m.ContactPreference = ContactPreference.Text;
                m.MobilePhoneNumber = null;
            });
        }

        [Test]
        public void Start_NationalInsuranceNumber_FormattedCorrectly()
        {
            ShouldBeValid(m => m.NationalInsuranceNumber = "AB 12 34 56 C", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ShouldBeValid(m => m.NationalInsuranceNumber = "ab 12 34 56 c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ShouldBeValid(m => m.NationalInsuranceNumber = "Ab123456c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));
            ShouldBeValid(m => m.NationalInsuranceNumber = "AB/123456/c", m => m.NationalInsuranceNumber.Should().Be("AB 12 34 56 C"));

            ShouldBeInvalid(m => m.NationalInsuranceNumber = "A 12 34 56 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 34 56 CD");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12/34/56 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "A. 12 34 56 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "AB .2 34 56 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 .4 56 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "AB 12 34 .6 C");
            ShouldBeInvalid(m => m.NationalInsuranceNumber = "A5 12 34 56 .");
        }

        #region test helpers

        protected void ShouldBeValid(Action<AboutYou> mutator, Action<AboutYou> postVerify = null)
        {
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.DoesNotThrow(() => BestStartGrant.Start(aboutYou), mutator.ToString());

            if (postVerify != null)
                postVerify(aboutYou);
        }

        protected void ShouldBeInvalid(Action<AboutYou> mutator)
        {
            var aboutYou = AboutYouBuilder.NewValid(mutator);
            Assert.Throws<DomainException>(() => BestStartGrant.Start(aboutYou), mutator.ToString());
        }

        #endregion
    }
}
