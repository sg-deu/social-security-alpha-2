using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_ApplicantDetailsTests : DomainTest
    {
        [Test]
        public void ApplicantDetails_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            ApplicantDetailsShouldBeValid(form, m => { });
            ApplicantDetailsShouldBeValid(form, m => m.Title = null);
            ApplicantDetailsShouldBeValid(form, m => m.Address.Line2 = null);
            ApplicantDetailsShouldBeValid(form, m => m.Address.Line3 = null);
            ApplicantDetailsShouldBeValid(form, m => m.MobilePhoneNumber = null);
            ApplicantDetailsShouldBeValid(form, m => m.HomePhoneNumer = null);
            ApplicantDetailsShouldBeValid(form, m => m.EmailAddress = null);

            ApplicantDetailsShouldBeInvalid(form, m => m.FullName = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.Address.Line1 = null);
            ApplicantDetailsShouldBeInvalid(form, m => m.Address.Postcode = null);
        }

        protected void ApplicantDetailsShouldBeValid(ChangeOfCircs form, Action<ApplicantDetails> mutator)
        {
            ShouldBeValid(() => form.AddApplicantDetails(ApplicantDetailsBuilder.NewValid(mutator)));
        }

        protected void ApplicantDetailsShouldBeInvalid(ChangeOfCircs form, Action<ApplicantDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddApplicantDetails(ApplicantDetailsBuilder.NewValid(mutator)));
        }
    }
}
