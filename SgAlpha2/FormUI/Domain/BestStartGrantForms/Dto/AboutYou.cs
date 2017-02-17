using System;
using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class AboutYou
    {
        public AboutYou()
        {
            CurrentAddress = new Address();
        }

        [DisplayName("Title (optional)")]
        public string               Title                   { get; set; }

        [DisplayName("First name")]
        public string               FirstName               { get; set; }

        [DisplayName("All other names (optional)")]
        public string               OtherNames              { get; set; }

        [DisplayName("Surname or family name")]
        public string               SurnameOrFamilyName     { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime?            DateOfBirth             { get; set; }

        [DisplayName("National Insurance number")]
        public string               NationalInsuranceNumber { get; set; }

        public Address              CurrentAddress          { get; set; }

        [DisplayName("Is this address Permanent or Temporary?")]
        public AddressStatus?       CurrentAddressStatus    { get; set; }

        [DisplayName("How do you want to be contacted?")]
        public ContactPreference?   ContactPreference       { get; set; }

        [DisplayName("Email address")]
        public string               EmailAddress            { get; set; }

        [DisplayName("Phone number")]
        public string               PhoneNumer              { get; set; }

        [DisplayName("Mobile phone number")]
        public string               MobilePhoneNumber       { get; set; }
    }
}