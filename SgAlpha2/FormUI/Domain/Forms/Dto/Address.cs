using System.ComponentModel;

namespace FormUI.Domain.Forms.Dto
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

        public static Address Copy(Address address)
        {
            if (address == null)
                return null;

            return new Address
            {
                Line1 =     address.Line1,
                Line2 =     address.Line2,
                Line3 =     address.Line3,
                Postcode =  address.Postcode,
            };
        }
    }
}