using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using System.Diagnostics.CodeAnalysis;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [ExcludeFromCodeCoverage]
    public class ExpectedChildrenBuilder
    {
        public static ExpectedChildren NewValid(Action<ExpectedChildren> mutator = null)
        {
            var value = new ExpectedChildren
            {
                ExpectancyDate = DateTime.Today + TimeSpan.FromDays(100),
                ExpectedBabyCount = 2,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
