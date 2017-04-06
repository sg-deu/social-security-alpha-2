using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.Forms.Dto
{
    // This is currently stored with the rest of the BSG data (unencrypted), so this will
    // possibly/probably be removed at a later stage
    // this is currently shared between BSG and CoC
    public class BankDetails
    {
        [DisplayName("Do you have a bank account?")]
        public bool?    HasBankAccount      { get; set; }

        [DisplayName("Name of the Account Holder")]
        [HintText("Write the name exactly as it appears on the bank card or on a letter from the bank.")]
        public string   NameOfAccountHolder { get; set; }

        [DisplayName("Full name of bank or building society")]
        public string   NameOfBank          { get; set; }

        [DisplayName("Sort Code")]
        [HintText("Tell us all 6 numbers for example 12-34-56.")]
        [UiLength(8)]
        [UiInputMask(InputMasks.SortCode)]
        public string   SortCode            { get; set; }

        [DisplayName("Account number")]
        [HintText("Most account numbers are 8 numbers long.")]
        [UiLength(10)]
        [UiInputMask(InputMasks.AccountNumber)]
        public string   AccountNumber       { get; set; }

        [DisplayName("Building Society roll or reference number")]
        [HintText("Most roll or reference numbers are made up of letters and numbers and are up to 18 characters long. If you’re not sure if the account has a roll or reference number, ask the building society.")]
        [UiLength(18)]
        public string   RollNumber          { get; set; }
    }
}