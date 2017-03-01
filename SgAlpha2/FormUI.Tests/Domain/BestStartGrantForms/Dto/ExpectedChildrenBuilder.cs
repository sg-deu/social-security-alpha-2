using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class ExpectedChildrenBuilder
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
    }
}
