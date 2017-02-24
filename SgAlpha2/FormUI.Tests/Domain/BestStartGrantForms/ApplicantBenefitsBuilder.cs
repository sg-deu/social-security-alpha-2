using System;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class ApplicantBenefitsBuilder
    {
        public static ApplicantBenefits NewValid(Part part, Action<ApplicantBenefits> mutator = null)
        {
            var value = new ApplicantBenefits
            {
                HasExistingBenefit = true,
            };

            if (part == Part.Part2)
            {
                value.ReceivingBenefitForUnder20 = true;
                value.YouOrPartnerInvolvedInTradeDispute = true;
            }

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
