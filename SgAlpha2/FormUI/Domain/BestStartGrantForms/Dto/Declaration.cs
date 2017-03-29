using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Declaration
    {
        [DisplayName("I agree that the information I’ve given is complete and correct, the Social Security Agency can check my information with other government departments.")]
        public bool AgreedToLegalStatement { get; set; }
    }
}