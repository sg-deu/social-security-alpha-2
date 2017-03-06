using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Address
    {
        [DisplayName("Street")]
        public string       Street1     { get; set; }

        [DisplayName("")]
        public string       Street2     { get; set; }

        [DisplayName("Town or City")]
        public string       TownOrCity  { get; set; }

        [DisplayName("Postcode")]
        public string       Postcode    { get; set; }
    }
}