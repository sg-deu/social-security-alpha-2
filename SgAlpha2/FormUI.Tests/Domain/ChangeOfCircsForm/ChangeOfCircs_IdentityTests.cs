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
                .Insert(f => f.ApplicantDetails.FirstName = "test name");

            var coc = new ChangeOfCircsBuilder("form").Insert();

            coc.AddIdentity("e.mail@test.com");

            coc.ExistingApplicantDetails.FirstName.Should().Be("test name");
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
