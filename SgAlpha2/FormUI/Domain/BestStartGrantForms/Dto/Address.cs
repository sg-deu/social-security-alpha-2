using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Address
    {
        [DisplayName("Address line 1")]
        public string Line1     { get; set; }

        [DisplayName("Address line 2")]
        public string Line2     { get; set; }

        [DisplayName("Address line 3")]
        public string Line3     { get; set; }

        [DisplayName("Postcode")]
        public string Postcode  { get; set; }
    }
}