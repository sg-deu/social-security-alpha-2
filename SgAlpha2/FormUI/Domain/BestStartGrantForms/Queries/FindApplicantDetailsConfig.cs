using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Queries
{
    public class FindApplicantDetailsConfig : Query<ApplicantDetailsConfig>
    {
        public ApplicantDetails ApplicantDetails;

        public override ApplicantDetailsConfig Find()
        {
            return new ApplicantDetailsConfig
            {
                ShouldAskCareQuestion               = BestStartGrant.ShouldAskCareQuestion(ApplicantDetails),
                ShouldAskEducationQuestion          = BestStartGrant.ShouldAskEducationQuestion(ApplicantDetails),
                ShouldAskForNationalInsuranceNumber = BestStartGrant.ShouldAskForNationalInsuranceNumber(ApplicantDetails),
            };
        }
    }
}