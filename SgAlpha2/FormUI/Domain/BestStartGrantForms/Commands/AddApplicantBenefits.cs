using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddApplicantBenefits : Command<NextSection>
    {
        public string   FormId;
        public Benefits ApplicantBenefits;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddApplicantBenefits(ApplicantBenefits);
        }
    }
}