using System;
using FluentAssertions;
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

            ExpectedChildrenShouldBeValid(form, m => m.IsBabyExpected = false, m =>
            {
                m.ExpectancyDate.Should().BeNull();
                m.IsMoreThan1BabyExpected.Should().BeNull();
                m.ExpectedBabyCount.Should().BeNull();
            });

            ExpectedChildrenShouldBeValid(form, m => m.IsMoreThan1BabyExpected = false, m =>
            {
                m.ExpectedBabyCount.Should().BeNull();
            });

            ExpectedChildrenShouldBeValid(form, m => m.ExpectancyDate = TestNowUtc);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 2);
            ExpectedChildrenShouldBeValid(form, m => m.ExpectedBabyCount = 10);

            ExpectedChildrenShouldBeInvalid(form, m => { m.IsBabyExpected = true; m.ExpectancyDate = null; });
            ExpectedChildrenShouldBeInvalid(form, m => { m.IsBabyExpected = true; m.IsMoreThan1BabyExpected = null; });
            ExpectedChildrenShouldBeInvalid(form, m => { m.IsMoreThan1BabyExpected = true; m.ExpectedBabyCount = null; });
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectancyDate = TestNowUtc - TimeSpan.FromDays(1));
            ExpectedChildrenShouldBeInvalid(form, m => { m.ExpectancyDate = null; m.ExpectedBabyCount = 1; });
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 1);
            ExpectedChildrenShouldBeInvalid(form, m => m.ExpectedBabyCount = 11);
        }

        protected void ExpectedChildrenShouldBeValid(BestStartGrant form, Action<ExpectedChildren> mutator, Action<ExpectedChildren> postVerify = null)
        {
            var expectedChildren = ExpectedChildrenBuilder.NewValid(mutator);
            ShouldBeValid(() => form.AddExpectedChildren(expectedChildren));
            postVerify?.Invoke(expectedChildren);
        }

        protected void ExpectedChildrenShouldBeInvalid(BestStartGrant form, Action<ExpectedChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExpectedChildren(ExpectedChildrenBuilder.NewValid(mutator)));
        }
    }
}
