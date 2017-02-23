using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class AboutYou
    {
        public AboutYou()
        {
            CurrentAddress = new Address();
        }

        [DisplayName("Title")]
        public string               Title                   { get; set; }

        [DisplayName("First name")]
        public string               FirstName               { get; set; }

        [DisplayName("All other names")]
        public string               OtherNames              { get; set; }

        [DisplayName("Surname or family name")]
        public string               SurnameOrFamilyName     { get; set; }

        [DisplayName("Date of Birth")]
        [HintText("For example, 18 03 1980")]
        public DateTime?            DateOfBirth             { get; set; }

        [DisplayName("National Insurance number")]
        [HintText("It's on your National Insurance Card, benefit letter, payslip or P60.<br/>For example, 'VO 12 34 56 D'")]
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