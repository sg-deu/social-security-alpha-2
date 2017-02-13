﻿using System;
using System.Collections.Generic;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class AboutYou
    {
        public AboutYou()
        {
            PreviousAddresses = new List<Address>();
        }

        public string               Title                       { get; set; }

        public string               FirstName                   { get; set; }

        public string               OtherNames                  { get; set; }

        public string               SurnameOrFamilyName         { get; set; }

        public DateTime             DateOfBirth                 { get; set; }

        public string               NationalInsuranceNumberText { get; set; }

        public Address              CurrentAddress              { get; set; }

        public AddressStatus?       CurrentAddressStatus        { get; set; }

        public IList<Address>       PreviousAddresses           { get; set; }

        public ContactPreference?   ContactPreference           { get; set; }

        public string               EmailAddress                { get; set; }

        public string               PhoneNumer                  { get; set; }

        public string               MobilePhoneNumber           { get; set; }
    }
}