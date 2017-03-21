using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_IdentityTests : DomainTest
    {
        [Test]
        public void Identity_Validation()
        {
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
