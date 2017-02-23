﻿using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    // This is currently stored with the rest of the BSG data (unencrypted), so this will
    // possibly/probably be removed at a later stage
    public class PaymentDetails
    {
        [DisplayName("Do you lack a bank account and require us to contact you to arrange payment?")]
        public bool?    LackingBankAccount  { get; set; }

        [DisplayName("Name of the Account Holder")]
        [HintText("Please type the name of the account holder exactly as it is shown on the chequebook or statement")]
        public string   NameOfAccountHolder { get; set; }

        [DisplayName("Full name of bank or building society")]
        public string   NameOfBank          { get; set; }

        [DisplayName("Sort Code")]
        [HintText("Please tell us all 6 numbers, for example 12-34-56")]
        public string   SortCode            { get; set; }

        [DisplayName("Account number")]
        [HintText("Most account numbers are 8 numbers long; if your account number has fewer than 10 numbers, please fill in the numbers from the left")]
        public string   AccountNumber       { get; set; }

        [DisplayName("Building Society roll or reference number")]
        [HintText("If you are using a building society account number you may need to tell us a roll or reference number; this may be made up of letters and numbers, and may be up to 18 characters long; if you are not sure if the account has a roll or reference number, ask the building society")]
        public string   RollNumber          { get; set; }
    }
}