using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class DeclarationModel : NavigableModel
    {
        // GET
        public bool RequiresGuardianDeclaration { get; set; }

        // POST
        public Declaration Declaration;
    }
}