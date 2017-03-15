using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_ExpectedChildrenTests : DomainTest
    {
        [Test]
        public void AddExpectedChildren_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ExpectedChildrenShouldBeValid(form, m => { });
            ExpectedChildrenShouldBeValid(form, m => { m.ExpectancyDate = null; m.ExpectedBabyCount = null; });
            ExpectedChildrenShouldBeValid(form, m => { m.ExpectancyDate = TestNowUtc; m.ExpectedBabyCount = null; });
            ExpectedChildrenShouldBeValid(form, m => m.ExpectancyDate = TestNowUtc);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 1);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 10);

            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectancyDate = TestNowUtc - TimeSpan.FromDays(1));
            ExpectedChildrenShouldBeInvalid(form, m => { m.ExpectancyDate = null; m.ExpectedBabyCount = 1; });
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 0);
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 11);
        }

        protected void ExpectedChildrenShouldBeValid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            ShouldBeValid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }

        protected void ExpectedChildrenShouldBeInvalid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }
    }
}
