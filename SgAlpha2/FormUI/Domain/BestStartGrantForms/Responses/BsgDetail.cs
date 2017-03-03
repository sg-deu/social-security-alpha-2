using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Domain.BestStartGrantForms.Responses
{
    public class BsgDetail
    {
        public string Id;

        public Consent              Consent;
        public ApplicantDetails     ApplicantDetails;
        public ExpectedChildren     ExpectedChildren;
        public ExistingChildren     ExistingChildren;
        public ApplicantBenefits    ApplicantBenefits;
        public HealthProfessional   HealthProfessional;
        public PaymentDetails       PaymentDetails;
        public Declaration          Declaration;

        public Sections?            PreviousSection;
        public bool                 IsFinalSection;
    }
}