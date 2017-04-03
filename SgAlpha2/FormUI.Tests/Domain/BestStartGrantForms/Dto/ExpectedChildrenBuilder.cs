using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class ExpectedChildrenBuilder
    {
        public static ExpectedChildren NewValid(Action<ExpectedChildren> mutator = null)
        {
            var value = new ExpectedChildren
            {
                IsBabyExpected = true,
                ExpectancyDate = DomainRegistry.NowUtc().Date + TimeSpan.FromDays(100),
                IsMoreThan1BabyExpected = true,
                ExpectedBabyCount = 2,
            };

            if (mutator != null)
                mutator(value);

            return value;
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
