using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class UKVerifyBuilder
    {
        public static UKVerify NewValid(Action<UKVerify> mutator = null)
        {
            var value = new UKVerify
            {
                //AgreedToConsent no longer needed...so, what to test?
                AgreedToConsent = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
