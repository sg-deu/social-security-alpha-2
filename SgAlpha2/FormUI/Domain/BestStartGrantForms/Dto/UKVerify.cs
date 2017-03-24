using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class UKVerify
    {
        // [DisplayName("I agree that the GOV.UK Verify can use the data I have provided in this BSG application form for the purposes of processing my application as described above")]
        // No longer needed - need to rejig these UKVerify tests when have time
        public bool AgreedToConsent { get; set; }
    }
}