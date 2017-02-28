using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class ConsentBuilder
    {
        public static Consent NewValid(Action<Consent> mutator = null)
        {
            var value = new Consent
            {
                AgreedToConsent = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
