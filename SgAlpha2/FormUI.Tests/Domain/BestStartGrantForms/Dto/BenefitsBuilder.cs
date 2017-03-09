using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class BenefitsBuilder
    {
        public static Benefits NewValid(Action<Benefits> mutator = null)
        {
            var value = new Benefits
            {
                HasExistingBenefit = YesNoDk.Yes,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
