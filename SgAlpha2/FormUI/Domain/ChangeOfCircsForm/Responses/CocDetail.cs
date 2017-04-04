using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Domain.ChangeOfCircsForm.Responses
{
    public class CocDetail
    {
        public string Id;

        public Consent          Consent;
        public string           Identity;
        public Options          Options;
        public ApplicantDetails ApplicantDetails;
        public PaymentDetails   PaymentDetails;
        public Evidence         Evidence;
        public Declaration      Declaration;

        public Sections?    PreviousSection;
        public bool         IsFinalSection;
    }
}