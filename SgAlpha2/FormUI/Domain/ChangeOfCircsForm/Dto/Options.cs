using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.ChangeOfCircsForm.Dto
{
    public class Options
    {
        [DisplayName("Personal details")]
        public bool     ChangePersonalDetails   { get; set; }

        [DisplayName("Partner's details")]
        public bool     ChangePartnerDetails    { get; set; }

        [DisplayName("Children's details")]
        public bool     ChangeChildrenDetails   { get; set; }

        [DisplayName("Payment details")]
        public bool     ChangePaymentDetails    { get; set; }

        [DisplayName("Anything else")]
        public bool     Other                   { get; set; }

        [DisplayName("Anything else")]
        [UIHint("Enter what you want to change in the comment box below.")]
        [UiLength(400)]
        public string   OtherDetails            { get; set; }
    }
}