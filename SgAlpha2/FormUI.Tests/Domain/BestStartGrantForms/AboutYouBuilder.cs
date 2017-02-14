using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class AboutYouBuilder
    {
        public static AboutYou NewValidAboutYou(Action<AboutYou> mutator = null)
        {
            var aboutYou = new AboutYou
            {
                Title = "test title",
                FirstName = "test first name",
                OtherNames = "test other names",
                SurnameOrFamilyName = "test surname or family name",
                DateOfBirth = new DateTime(1980, 12, 11),
                NationalInsuranceNumberText = "AB123456C",
                CurrentAddress = new Address
                {
                    Street1 = "test street 1",
                    Street2 = "test street 2",
                    TownOrCity = "test town or city",
                    Postcode = "test postcode",
                    DateMovedIn = DateTime.Today - TimeSpan.FromDays(365),
                },
                CurrentAddressStatus = AddressStatus.Permanent,
                ContactPreference = ContactPreference.Email,
                EmailAddress = "test.unit@unit.test",
                PhoneNumer = "01234 567 890",
                MobilePhoneNumber = "12345 678 901",
            };

            if (mutator != null)
                mutator(aboutYou);

            return aboutYou;
        }
    }
}
