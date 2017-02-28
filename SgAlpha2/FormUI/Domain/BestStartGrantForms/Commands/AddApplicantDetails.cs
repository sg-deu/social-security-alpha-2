using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddApplicantDetails : Command
    {
        public string           FormId;
        public ApplicantDetails ApplicantDetails;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddApplicantDetails(ApplicantDetails);
        }
    }
}