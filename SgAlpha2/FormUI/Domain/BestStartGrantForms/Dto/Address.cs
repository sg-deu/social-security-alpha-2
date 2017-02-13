using System;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Address
    {
        public string   Street1     { get; set; }

        public string   Street2     { get; set; }

        public string   TownOrCity  { get; set; }

        public string   Postcode    { get; set; }

        public DateTime DateMovedIn { get; set; }
    }
}