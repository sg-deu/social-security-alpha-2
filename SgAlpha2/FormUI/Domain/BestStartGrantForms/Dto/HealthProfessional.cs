using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class HealthProfessional
    {
        [DisplayName("Health Practitioners GMC No. or NMC pin")]
        [UiLength(20)]
        public string Pin { get; set; }
    }
}