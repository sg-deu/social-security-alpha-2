﻿using FormUI.Domain.Forms.Dto;

namespace FormUI.Tests.Domain.Forms.Dto
{
    public class BankDetailsBuilder
    {
        public static T Populate<T>(T value)
            where T : BankDetails
        {
            value.HasBankAccount = true;
            value.NameOfAccountHolder = "unit testster";
            value.NameOfBank = "unit test bank";
            value.SortCode = "01-02-03";
            value.AccountNumber = "00112233";
            value.RollNumber = "12/3";
            return value;
        }
    }
}
