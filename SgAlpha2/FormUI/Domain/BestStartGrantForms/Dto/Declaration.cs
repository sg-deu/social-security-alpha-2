using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Declaration
    {
        [DisplayName("I agree that the information I’ve given is complete and correct, the Social Security Agency can check my information with other government departments, I’ll pay back any money I’ve been overpaid and I’ll tell you if anything changes")]
        public bool AgreedToLegalStatement { get; set; }
    }
}