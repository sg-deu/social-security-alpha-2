using FormUI.Controllers.Shared;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Controllers.Coc
{
    public class DeclarationModel : NavigableModel
    {
        // GET
        public bool RequiresGuardianDeclaration { get; set; }

        // POST
        public Declaration Declaration;
    }
}