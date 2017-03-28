using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Benefits
    {
        [DisplayName("Income Support")]
        public bool HasIncomeSupport                                { get; set; }

        [DisplayName("Income - based Jobseekers Allowance(JSA)")]
        public bool HasIncomeBasedJobseekersAllowance               { get; set; }

        [DisplayName("Income-related Employment and Support Allowance (ESA)")]
        public bool HasIncomeRelatedEmplymentAndSupportAllowance    { get; set; }

        [DisplayName("Universal Credit")]
        public bool HasUniversalCredit                              { get; set; }

        [DisplayName("Child Tax Credit")]
        public bool HasChildTaxCredit                               { get; set; }

        [DisplayName("Working Tax Credit")]
        public bool HasWorkingTextCredit                            { get; set; }

        [DisplayName("Housing Benefit")]
        public bool HasHousingBenefit                               { get; set; }

        [DisplayName("Pension Credit")]
        public bool HasPensionCredit                                { get; set; }


        [DisplayName("None of the above")]
        public bool None                                            { get; set; }

        [DisplayName("Don't know")]
        public bool Unknown                                         { get; set; }

        public YesNoDk? HasExistingBenefit()
        {
            var hasBenefit = HasIncomeSupport
                || HasIncomeBasedJobseekersAllowance
                || HasIncomeRelatedEmplymentAndSupportAllowance
                || HasUniversalCredit
                || HasChildTaxCredit
                || HasWorkingTextCredit
                || HasHousingBenefit
                || HasPensionCredit;

            if (hasBenefit)
                return YesNoDk.Yes;

            if (None) return YesNoDk.No;
            if (Unknown) return YesNoDk.DontKnow;

            return null;
        }
    }
}