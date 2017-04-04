using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_IdentityTests : DomainTest
    {
        [Test]
        public void AddIdentity_StoresBsgDetail()
        {
            var existingForm = new BestStartGrantBuilder("existing_form")
                .WithCompletedSections()
                .With(f => f.UserId, "e.mail@test.com")
                .Insert(f =>
                {
                    f.ApplicantDetails.Title = "tst_title";
                    f.ApplicantDetails.FirstName = "tst_fn";
                    f.ApplicantDetails.OtherNames = "tst_on";
                    f.ApplicantDetails.SurnameOrFamilyName = "tst_sn";
                    f.ApplicantDetails.CurrentAddress.Line1 = "al1";
                    f.ApplicantDetails.MobilePhoneNumber = "123";
                    f.ApplicantDetails.PhoneNumer = "234";
                    f.ApplicantDetails.EmailAddress = "t.t@t.t";

                    f.PaymentDetails.HasBankAccount = true;
                    f.PaymentDetails.AccountNumber = "12345";
                    f.PaymentDetails.SortCode = "12-34-56";
                });

            var coc = new ChangeOfCircsBuilder("form").Insert();

            coc.AddIdentity("e.mail@test.com");

            coc.ExistingApplicantDetails.Title.Should().Be("tst_title");
            coc.ExistingApplicantDetails.FullName.Should().Be("tst_fn tst_on tst_sn");
            coc.ExistingApplicantDetails.Address.Line1.Should().Be("al1");
            coc.ExistingApplicantDetails.MobilePhoneNumber.Should().Be("123");
            coc.ExistingApplicantDetails.HomePhoneNumer.Should().Be("234");
            coc.ExistingApplicantDetails.EmailAddress.Should().Be("t.t@t.t");

            coc.ExistingApplicantDetails.ShouldBeEquivalentTo(coc.ApplicantDetails);

            coc.ExistingPaymentDetails.HasBankAccount.Should().BeTrue();
            coc.ExistingPaymentDetails.AccountNumber.Should().BeNull();
            coc.ExistingPaymentDetails.SortCode.Should().BeNull();

            coc.ExistingPaymentDetails.ShouldBeEquivalentTo(coc.PaymentDetails);
        }

        [Test]
        public void AddIdentity_Throws_WhenNoExistingFormForIdentity()
        {
            var coc = new ChangeOfCircsBuilder("form").Insert();

            Assert.Throws<DomainException>(() =>
                coc.AddIdentity("does.not@exist.com"));
        }

        [Test]
        public void Identity_Validation()
        {
            new BestStartGrantBuilder("form").PreviousApplicationFor("user.name@mail.com").Insert();
            var form = new ChangeOfCircsBuilder("form").Insert();

            IdentityShouldBeValid(form, "user.name@mail.com");

            IdentityShouldBeInvalid(form, null);
            IdentityShouldBeInvalid(form, "");
        }

        protected void IdentityShouldBeValid(ChangeOfCircs form, string userId)
        {
            ShouldBeValid(() => form.AddIdentity(userId));
        }

        protected void IdentityShouldBeInvalid(ChangeOfCircs form, string userId)
        {
            ShouldBeInvalid(() => form.AddIdentity(userId));
        }
    }
}
