using System;
using System.Collections.Generic;

namespace FormUI.Controllers.Bsg
{
    public class AboutYouModel
    {
        public AboutYouModel()
        {
            PreviousAddresses = new List<AddressModel>();
        }

        public string           Title                       { get; set; }

        public string           FirstName                   { get; set; }

        public string           OtherNames                  { get; set; }

        public string           SurnameOrFamilyName         { get; set; }

        public DateTime         DateOfBirth                 { get; set; }

        public string           NationalInsuranceNumberText { get; set; }

        public AddressModel     CurrentAddress              { get; set; }

        public AddressStatus?   CurrentAddressStatus   { get; set; }

        public IList<AddressModel> PreviousAddresses { get; set; }
    }

    public class AddressModel
    {
        public string           Street1     { get; set; }

        public string           Street2     { get; set; }

        public string           TownOrCity  { get; set; }

        public string           Postcode    { get; set; }

        public DateTime         DateMovedIn { get; set; }
    }

    public enum AddressStatus
    {
        Permanent,
        Temporary,
    }
}