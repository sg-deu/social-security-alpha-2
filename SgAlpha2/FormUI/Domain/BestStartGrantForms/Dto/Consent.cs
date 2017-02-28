using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Consent
    {
        [DisplayName("I agree that the Social Security Agency can use the data I have provided in this BSG application form for the purposes of processing my application as described above")]
        public bool AgreedToConsent { get; set; }
    }
}