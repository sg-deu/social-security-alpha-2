using System.ComponentModel;

namespace FormUI.Domain.ChangeOfCircsForm.Dto
{
    public class Declaration
    {
        [DisplayName("I agree that the Social Security Agency can use the data I have provided in this Change of Circumstances application form for the purposes of processing my application as described above")]
        public bool AgreedToLegalStatement { get; set; }
    }
}