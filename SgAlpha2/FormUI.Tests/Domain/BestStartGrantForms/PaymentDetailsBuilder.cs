using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class PaymentDetailsBuilder
    {
        public static PaymentDetails NewValid(Action<PaymentDetails> mutator = null)
        {
            var value = new PaymentDetails
            {
                LackingBankAccount = false,
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
