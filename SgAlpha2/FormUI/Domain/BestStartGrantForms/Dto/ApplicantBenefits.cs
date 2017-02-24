using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ApplicantBenefits
    {
        [DisplayName("Are you on any of the following benefits?")]
        // hint-text removed until logic is implemented: [HintText("If you say no to this question, the next question will be about partners benefit")]
        public bool? HasExistingBenefit                 { get; set; }

        [DisplayName("Are you receiving benefit for the parent of the baby, or an expectant mother, because they are under 20 years of age?")]
        public bool? ReceivingBenefitForUnder20         { get; set; }

        [DisplayName("Are you or your partner involved in a trade dispute?")]
        [HintText("We use trade dispute to mean a strike, a walkout, a lockout or another dispute at work")]
        public bool? YouOrPartnerInvolvedInTradeDispute { get; set; }
    }
}