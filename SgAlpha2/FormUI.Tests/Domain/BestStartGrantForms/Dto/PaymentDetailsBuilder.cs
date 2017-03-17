﻿using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class PaymentDetailsBuilder
    {
        public static PaymentDetails NewValid(Action<PaymentDetails> mutator = null)
        {
            var value = new PaymentDetails
            {
                HasBankAccount = true,
                NameOfAccountHolder = "unit testster",
                NameOfBank = "unit test bank",
                SortCode = "01-02-03",
                AccountNumber = "00112233",
                RollNumber = "12/3",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
