using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.Forms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class ApplicantDetailsBuilder
    {
        public static ApplicantDetails NewValid(Action<ApplicantDetails> mutator = null)
        {
            var value = new ApplicantDetails
            {
                Title = "test title",
                FirstName = "test first name",
                OtherNames = "test other names",
                SurnameOrFamilyName = "test surname or family name",
                DateOfBirth = DomainRegistry.NowUtc().Date.AddYears(-19),
                PreviouslyLookedAfter = false,
                FullTimeEducation = false,
                NationalInsuranceNumber = "AB123456C",
                CurrentAddress = AddressBuilder.NewValid("applicant"),
                DateMovedIn = DomainRegistry.NowUtc().Date - TimeSpan.FromDays(365),
                EmailAddress = "test.unit@unit.test",
                PhoneNumer = "01234 567 890",
                MobilePhoneNumber = "12345 678 901",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static ApplicantDetails Under16(this ApplicantDetails applicantDetails, DateTime nowUtc)
        {
            return applicantDetails.Aged(nowUtc, 15);
        }

        public static ApplicantDetails Aged16(this ApplicantDetails applicantDetails, DateTime nowUtc)
        {
            return applicantDetails.Aged(nowUtc, 16);
        }

        public static ApplicantDetails Over25(this ApplicantDetails applicantDetails, DateTime nowUtc)
        {
            return applicantDetails.Aged(nowUtc, 28);
        }

        public static ApplicantDetails Under25CareLeaver(this ApplicantDetails applicantDetails, DateTime nowUtc)
        {
            applicantDetails.Aged(nowUtc, 23);
            applicantDetails.PreviouslyLookedAfter = true;
            return applicantDetails;
        }

        public static ApplicantDetails Aged(this ApplicantDetails applicantDetails, DateTime nowUtc, int age)
        {
            applicantDetails.DateOfBirth = nowUtc.ToLocalTime().Date.AddYears(-age);
            return applicantDetails;
        }

        public static ApplicantDetails PartOfGuardianBenefits(this ApplicantDetails applicantDetails, DateTime nowUtc)
        {
            applicantDetails.Aged(nowUtc, 18);
            applicantDetails.FullTimeEducation = true;
            return applicantDetails;
        }
    }
}
