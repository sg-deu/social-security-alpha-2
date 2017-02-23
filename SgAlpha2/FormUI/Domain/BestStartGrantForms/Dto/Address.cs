using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

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

        [DisplayName("Date You or your Partner moved into this address")]
        [HintText("For example, 01 01 2012")]
        public DateTime?    DateMovedIn { get; set; }
    }
}