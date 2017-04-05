using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Domain.BestStartGrantForms.Responses
{
    public class BsgDetail
    {
        public string Id;

        public Consent              Consent;
        public UKVerify             UKVerify;
        public ApplicantDetails     ApplicantDetails;
        public ExpectedChildren     ExpectedChildren;
        public ExistingChildren     ExistingChildren;
        public Benefits             ApplicantBenefits;
        public Benefits             PartnerBenefits;
        public Benefits             GuardianBenefits;
        public Benefits             GuardianPartnerBenefits;
        public RelationDetails      PartnerDetails;
        public RelationDetails      GuardianDetails;
        public RelationDetails      GuardianPartnerDetails;
        public HealthProfessional   HealthProfessional;
        public PaymentDetails       PaymentDetails;
        public Declaration          Declaration;

        public Sections?            PreviousSection;
        public bool                 IsFinalSection;
    }
}