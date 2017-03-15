using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_ApplicantDetailsTests : DomainTest
    {
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
    }
}
