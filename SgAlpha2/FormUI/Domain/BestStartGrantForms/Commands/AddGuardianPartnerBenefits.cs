using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddGuardianPartnerBenefits : Command<NextSection>
    {
        public string   FormId;
        public Benefits GuardianPartnerBenefits;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddGuardianPartnerBenefits(GuardianPartnerBenefits);
        }
    }
}