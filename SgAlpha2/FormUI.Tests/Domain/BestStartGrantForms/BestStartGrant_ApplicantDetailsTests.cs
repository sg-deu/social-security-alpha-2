﻿using System;
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
        public void ApplicantDetails_UsesEmailToIdentifyUser()
        {
            var form = new BestStartGrantBuilder("form").Insert();
            var details = ApplicantDetailsBuilder.NewValid(d => d.EmailAddress = "test.email");

            form.AddApplicantDetails(details);

            form.UserId.Should().Be("test.email");
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
        public void ApplicantDetails_RequiresNationalInsuranceNumber()
        {
            TestNowUtc = new DateTime(2009, 08, 07, 06, 05, 04);
            var applicantDetails = ApplicantDetailsBuilder.NewValid();

            BestStartGrant.ShouldAskForNationalInsuranceNumber(applicantDetails).Should().BeTrue("default builder should ask question");

            applicantDetails.DateOfBirth = null;
            BestStartGrant.ShouldAskForNationalInsuranceNumber(applicantDetails).Should().BeTrue("should ask by default");

            // applicant is 16 today
            applicantDetails.DateOfBirth = new DateTime(1993, 08, 07);
            BestStartGrant.ShouldAskForNationalInsuranceNumber(applicantDetails).Should().BeTrue("ask for NINO if >= 16");

            // applicant is 16 tomorrow (still 15)
            applicantDetails.DateOfBirth = new DateTime(1993, 08, 08);
            BestStartGrant.ShouldAskForNationalInsuranceNumber(applicantDetails).Should().BeFalse("under 16 does not have NINO");
        }

        [Test]
        public void AddApplicantDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m => { });
            ApplicantDetailsShouldBeValid(form, m => m.Title = null);
            ApplicantDetailsShouldBeValid(form, m => m.OtherNames = null);
            ApplicantDetailsShouldBeValid(form, m => m.CurrentAddress.Line2 = null);
            ApplicantDetailsShouldBeValid(form, m => m.CurrentAddress.Line3 = null);
            ApplicantDetailsShouldBeValid(form, m => m.DateOfBirth = TestNowUtc - TimeSpan.FromDays(1));
            ApplicantDetailsShouldBeValid(form, m => { m.Over25(TestNowUtc.Value); m.PreviouslyLookedAfter = null; });
            ApplicantDetailsShouldBeValid(form, m => { m.Over25(TestNowUtc.Value); m.FullTimeEducation = null; });
            ApplicantDetailsShouldBeValid(form, m => { m.Under16(TestNowUtc.Value); m.NationalInsuranceNumber = null; });

            ApplicantDetailsShouldBeValid(form, m => { m.Over25(TestNowUtc.Value); m.PreviouslyLookedAfter = true; }, m => m.PreviouslyLookedAfter.Should().BeNull());
            ApplicantDetailsShouldBeValid(form, m => { m.Over25(TestNowUtc.Value); m.FullTimeEducation = true; }, m => m.FullTimeEducation.Should().BeNull());
            ApplicantDetailsShouldBeValid(form, m => { m.Under16(TestNowUtc.Value); m.NationalInsuranceNumber = "AB 12 34 56 C"; }, m => m.NationalInsuranceNumber.Should().BeNull());

            ApplicantDetailsShouldBeInvalid(form, m => m.FirstName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.SurnameOrFamilyName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.DateOfBirth = TestNowUtc);
            ApplicantDetailsShouldBeInvalid(form, m => m.PreviouslyLookedAfter = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.FullTimeEducation = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Line1 = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.CurrentAddress.Postcode = null);
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
            postVerify?.Invoke(applicantDetails);
        }

        protected void ApplicantDetailsShouldBeInvalid(BestStartGrant form, Action<ApplicantDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantDetails(ApplicantDetailsBuilder.NewValid(mutator)));
        }
    }
}
