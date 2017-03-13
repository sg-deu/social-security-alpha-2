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
                ExpectancyDate = DomainRegistry.NowUtc().Date + TimeSpan.FromDays(100),
                ExpectedBabyCount = 2,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static ExpectedChildren NoBabyExpected(this ExpectedChildren expectedChildren)
        {
            expectedChildren.ExpectedBabyCount = null;
            return expectedChildren;
        }

        public static ExpectedChildren ExpectedBabyCount(this ExpectedChildren expectedChildren, int? babyCount)
        {
            expectedChildren.ExpectedBabyCount = babyCount;
            return expectedChildren;
        }
    }
}
