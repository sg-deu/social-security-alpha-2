using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class ExpectedChildrenBuilder
    {
        public static ExpectedChildren NewValid(Action<ExpectedChildren> mutator = null)
        {
            return Populate(new ExpectedChildren(), mutator);
        }

        public static ExpectedChildren Populate(ExpectedChildren expectedChildren, Action<ExpectedChildren> mutator = null)
        {
            expectedChildren.IsBabyExpected = true;
            expectedChildren.ExpectancyDate = DomainRegistry.NowUtc().Date + TimeSpan.FromDays(100);
            expectedChildren.IsMoreThan1BabyExpected = true;
            expectedChildren.ExpectedBabyCount = 2;

            mutator?.Invoke(expectedChildren);

            return expectedChildren;
        }

        public static ExpectedChildren NoBabyExpected(this ExpectedChildren expectedChildren)
        {
            expectedChildren.IsBabyExpected = false;
            expectedChildren.ExpectancyDate = null;
            expectedChildren.ExpectedBabyCount = null;
            return expectedChildren;
        }

        public static ExpectedChildren ExpectedBabyCount(this ExpectedChildren expectedChildren, int? babyCount)
        {
            expectedChildren.IsBabyExpected = babyCount > 0;
            expectedChildren.ExpectancyDate = DomainRegistry.NowUtc().Date.AddDays(100);
            expectedChildren.IsMoreThan1BabyExpected = babyCount > 1;
            expectedChildren.ExpectedBabyCount = babyCount > 1 ? babyCount.Value : (int?)null;
            return expectedChildren;
        }
    }
}
