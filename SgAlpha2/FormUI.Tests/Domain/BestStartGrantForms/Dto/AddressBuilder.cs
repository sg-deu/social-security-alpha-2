using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class AddressBuilder
    {
        public static Address NewValid(Action<Address> mutator = null)
        {
            var value = new Address
            {
                Line1 = "test address line 1",
                Line2 = "test address line 2",
                Line3 = "test address line 3",
                Postcode = "test postcode",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
