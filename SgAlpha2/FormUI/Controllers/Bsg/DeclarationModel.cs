using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class DeclarationModel : NavigableModel
    {
        // GET
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
        public Evidence             Evidence;

        public bool             RequiresGuardianDeclaration;

        // POST
        public Declaration Declaration;
    }
}