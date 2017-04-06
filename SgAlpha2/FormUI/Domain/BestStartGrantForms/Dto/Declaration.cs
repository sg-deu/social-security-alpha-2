using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Declaration
    {
        [DisplayName("I agree with the 4 statements.")]
        public bool AgreedToLegalStatement { get; set; }

        public bool RequiresGuardianDeclaration { get; set; }
    }
}