﻿using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Forms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class PaymentDetailsBuilder
    {
        public static PaymentDetails NewValid(Action<PaymentDetails> mutator = null)
        {
            var value = BankDetailsBuilder.Populate(new PaymentDetails());

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
