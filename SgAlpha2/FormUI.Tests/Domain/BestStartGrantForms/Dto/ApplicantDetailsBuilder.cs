using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class ApplicantDetailsBuilder
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
                CurrentAddress = new Address
                {
                    Street1 = "test street 1",
                    Street2 = "test street 2",
                    TownOrCity = "test town or city",
                    Postcode = "test postcode",
                },
                DateMovedIn = DomainRegistry.NowUtc().Date - TimeSpan.FromDays(365),
                CurrentAddressStatus = AddressStatus.Permanent,
                ContactPreference = ContactPreference.Email,
                EmailAddress = "test.unit@unit.test",
                PhoneNumer = "01234 567 890",
                MobilePhoneNumber = "12345 678 901",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
