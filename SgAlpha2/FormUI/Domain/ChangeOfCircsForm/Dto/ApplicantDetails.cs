using System.ComponentModel;
using FormUI.Domain.Forms.Dto;

namespace FormUI.Domain.ChangeOfCircsForm.Dto
{
    public class ApplicantDetails
    {
        public ApplicantDetails()
        {
            Address = new Address();
        }

        [DisplayName("Title")]
        public string               Title                   { get; set; }

        [DisplayName("Full name")]
        public string               FullName                { get; set; }

        public Address              Address                 { get; set; }

        [DisplayName("Mobile phone number")]
        public string               MobilePhoneNumber       { get; set; }

        [DisplayName("Home phone number")]
        public string               HomePhoneNumer          { get; set; }

        [DisplayName("Email address")]
        public string               EmailAddress            { get; set; }
    }
}