using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddApplicantDetails : Command<NextSection>
    {
        public string           FormId;
        public ApplicantDetails ApplicantDetails;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddApplicantDetails(ApplicantDetails);
        }
    }
}