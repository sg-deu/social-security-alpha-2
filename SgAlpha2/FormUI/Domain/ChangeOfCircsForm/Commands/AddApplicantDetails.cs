using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddApplicantDetails : Command<NextSection>
    {
        public string           FormId;
        public ApplicantDetails ApplicantDetails;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddApplicantDetails(ApplicantDetails);
        }
    }
}