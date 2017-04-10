using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class DeclarationModel : NavigableModel
    {
        // GET
        public ApplicantDetails ApplicantDetails;
        public bool             RequiresGuardianDeclaration;

        // POST
        public Declaration Declaration;
    }
}