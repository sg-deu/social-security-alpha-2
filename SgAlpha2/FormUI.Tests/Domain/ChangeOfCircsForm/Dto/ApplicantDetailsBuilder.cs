using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Forms.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
{
    public static class ApplicantDetailsBuilder
    {
        public static ApplicantDetails NewValid(Action<ApplicantDetails> mutator = null)
        {
            var value = new ApplicantDetails
            {
                Title = "test title",
                FullName = "test full name",
                Address = AddressBuilder.NewValid("applicant"),
                MobilePhoneNumber = "12345 678 901",
                HomePhoneNumer = "01234 567 890",
                EmailAddress = "test.unit@unit.test",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
