using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
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
    }
}
