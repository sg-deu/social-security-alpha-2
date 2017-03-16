using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class AddressBuilder
    {
        public static Address NewValid(string owner = "unit", Action<Address> mutator = null)
        {
            var value = new Address
            {
                Line1 = $"{owner} test address line 1",
                Line2 = $"{owner} test address line 2",
                Line3 = $"{owner} test address line 3",
                Postcode = $"{owner} test postcode",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
