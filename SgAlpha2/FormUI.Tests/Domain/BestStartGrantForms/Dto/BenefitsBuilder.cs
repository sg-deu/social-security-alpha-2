using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class BenefitsBuilder
    {
        public static Benefits NewEmpty(Action<Benefits> mutator = null)
        {
            var value = new Benefits();

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Benefits NewWithBenefit(Action<Benefits> mutator = null)
        {
            var value = new Benefits
            {
                HasIncomeSupport = true,
                HasIncomeBasedJobseekersAllowance = true,
                HasIncomeRelatedEmplymentAndSupportAllowance = true,
                HasUniversalCredit = true,
                HasChildTaxCredit = true,
                HasWorkingTextCredit = true,
                HasHousingBenefit = true,
                HasPensionCredit = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Benefits NewUnknown(Action<Benefits> mutator = null)
        {
            var value = new Benefits
            {
                Unknown = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Benefits NewNone(Action<Benefits> mutator = null)
        {
            var value = new Benefits
            {
                None = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Benefits SetEmpty(this Benefits benefits, Action<Benefits> mutator = null)
        {
            benefits.HasIncomeSupport = false;
            benefits.HasIncomeBasedJobseekersAllowance = false;
            benefits.HasIncomeRelatedEmplymentAndSupportAllowance = false;
            benefits.HasUniversalCredit = false;
            benefits.HasChildTaxCredit = false;
            benefits.HasWorkingTextCredit = false;
            benefits.HasHousingBenefit = false;
            benefits.HasPensionCredit = false;

            benefits.Unknown = false;
            benefits.None = false;

            if (mutator != null)
                mutator(benefits);

            return benefits;
        }

        public static Benefits WithBenefit(this Benefits benefits, Action<Benefits> mutator = null)
        {
            benefits = benefits.SetEmpty(b => b.HasIncomeSupport = true);
            mutator?.Invoke(benefits);
            return benefits;
        }

        public static Benefits None(this Benefits benefits, Action<Benefits> mutator = null)
        {
            benefits = benefits.SetEmpty(b => b.None = true);
            mutator?.Invoke(benefits);
            return benefits;
        }

        public static Benefits Unknown(this Benefits benefits, Action<Benefits> mutator = null)
        {
            benefits = benefits.SetEmpty(b => b.Unknown = true);
            mutator?.Invoke(benefits);
            return benefits;
        }
    }
}
