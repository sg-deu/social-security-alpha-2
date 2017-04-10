using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class DeclarationModel : NavigableModel
    {
        // GET
        public ApplicantDetails ApplicantDetails;
        public ExpectedChildren ExpectedChildren;
        public ExistingChildren ExistingChildren;
        public Benefits         ApplicantBenefits;
        public bool             RequiresGuardianDeclaration;

        // POST
        public Declaration Declaration;
    }
}