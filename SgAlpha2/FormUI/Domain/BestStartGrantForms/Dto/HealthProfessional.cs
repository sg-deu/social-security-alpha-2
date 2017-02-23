using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class HealthProfessional
    {
        [DisplayName("Health Practitioners GMC No. or NMC pin")]
        [StringLength(20, ErrorMessage = "Please supply a GMC No. or NMC pin that is no more than 20 characters")]
        public string Pin { get; set; }
    }
}