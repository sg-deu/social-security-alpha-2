using System;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_ExpectedChildrenTests : DomainTest
    {
        [Test]
        public void ExpectedChildren_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            ExpectedChildrenShouldBeValid(form, m => { });

            ExpectedChildrenShouldBeInvalid(form, m => m.IsBabyExpected = false);
            ExpectedChildrenShouldBeInvalid(form, m => m.IsBabyExpected = null);
        }

        protected void ExpectedChildrenShouldBeValid(ChangeOfCircs form, Action<ExpectedChildren> mutator)
        {
            ShouldBeValid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }

        protected void ExpectedChildrenShouldBeInvalid(ChangeOfCircs form, Action<ExpectedChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }
    }
}
