using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
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
