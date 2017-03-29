using System;
using System.ComponentModel;
using FormUI.Domain.Forms.Dto;
using FormUI.Domain.Util;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ApplicantDetails : INationalInsuranceNumberHolder
    {
        public ApplicantDetails()
        {
            CurrentAddress = new Address();
        }

        [DisplayName("Title")]
        public string               Title                   { get; set; }

        [DisplayName("First name")]
        public string               FirstName               { get; set; }

        [DisplayName("All other names")]
        public string               OtherNames              { get; set; }

        [DisplayName("Last name")]
        public string               SurnameOrFamilyName     { get; set; }

        [DisplayName("Date of Birth")]
        [HintText("For example 01 03 1980")]
        public DateTime?            DateOfBirth             { get; set; }

        [DisplayName("Have you previously been in looked after care?")]
        public bool?                PreviouslyLookedAfter   { get; set; }

        [DisplayName("Are you in full time education, 18/19 and part of your parent's or guardian's benefit claim?")]
        public bool?                FullTimeEducation       { get; set; }

        [DisplayName("National Insurance number")]
        [HintText("You can find this on your National Insurance Card, benefit letter, payslip or P60.<br/>For example, 'VO 12 34 56 D'")]
        [UiInputMask(InputMasks.NationalInsuranceNumber)]
        public string               NationalInsuranceNumber { get; set; }

        public Address              CurrentAddress          { get; set; }

        [DisplayName("Date You or your Partner moved into this address")]
        [HintText("For example, 01 01 2012")]
        public DateTime?            DateMovedIn { get; set; }

        [DisplayName("Email address")]
        public string               EmailAddress            { get; set; }

        [DisplayName("Home Phone number")]
        public string               PhoneNumer              { get; set; }

        [DisplayName("Mobile phone number")]
        public string               MobilePhoneNumber       { get; set; }

        internal int? Age()
        {
            if (!DateOfBirth.HasValue)
                return null;

            var today = DomainRegistry.NowUtc().ToLocalTime().Date;
            var dob = DateOfBirth.Value;
            var age = today.Year - dob.Year;

            if (dob.AddYears(age) > today)
                age--; // birthday has not happened this year

            return age;
        }
    }
}