using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class AddApplicantBenefitsBuilder
    {
        public static ApplicantBenefits NewValidPart1(Action<ApplicantBenefits> mutator = null)
        {
            var value = new ApplicantBenefits
            {
                HasExistingBenefit = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static ApplicantBenefits NewValidPart2(Action<ApplicantBenefits> mutator = null)
        {
            var value = NewValidPart1(b =>
            {
                b.ReceivingBenefitForUnder20 = true;
                b.YouOrPartnerInvolvedInTradeDispute = true;
            });

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
