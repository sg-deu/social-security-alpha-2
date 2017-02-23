using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class HealthProfessional
    {
        [DisplayName("Health Practitioners GMC No. or NMC pin")]
        public string Pin { get; set; }
    }
}