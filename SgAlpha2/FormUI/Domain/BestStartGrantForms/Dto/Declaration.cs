using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Declaration
    {
        [DisplayName("I agree")]
        public bool AgreedToLegalStatement { get; set; }
    }
}