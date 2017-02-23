using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class HealthProfessionalBuilder
    {
        public static HealthProfessional NewValid(Action<HealthProfessional> mutator = null)
        {
            var value = new HealthProfessional
            {
                Pin = "GMC12345",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
