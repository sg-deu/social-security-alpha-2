using System;
using System.Diagnostics;
using FormUI.Domain.Forms.Dto;
using FormUI.Domain.Util;
using Newtonsoft.Json;

namespace FormUI.Domain.Forms
{
    public abstract class Form : IDocument
    {
        protected Form(string id)
        {
            Id = id;
            Started = DomainRegistry.NowUtc();
        }

        protected static IRepository Repository
        {
            [DebuggerStepThrough]
            get { return DomainRegistry.Repository; }
        }

        protected static ICloudStore CloudStore
        {
            [DebuggerStepThrough]
            get { return DomainRegistry.CloudStore; }
        }

        [JsonProperty(PropertyName = "id")]
        public string       Id          { get; protected set; }

        public string       UserId      { get; protected set; }
        public DateTime     Started     { get; protected set; }
        public DateTime?    Completed   { get; protected set; }

        protected void OnUserIdentified(string userId)
        {
            UserId = userId;
        }

        protected void OnBeforeUpdate(bool isFinalUpdate)
        {
            if (Completed.HasValue)
                throw new DomainException("This application has already been submitted and cannot be modified");

            if (isFinalUpdate)
                Completed = DomainRegistry.NowUtc();
        }

        protected static void Validate(BankDetails bankDetails)
        {
            var ctx = new ValidationContext<BankDetails>(bankDetails);

            ctx.Required(m => m.HasBankAccount, "Please indicate if you have a bank account");

            if (bankDetails.HasBankAccount == false)
            {
                bankDetails.NameOfAccountHolder = null;
                bankDetails.NameOfBank = null;
                bankDetails.SortCode = null;
                bankDetails.AccountNumber = null;
                bankDetails.RollNumber = null;
            }

            if (bankDetails.HasBankAccount == true)
            {
                ctx.Required(m => m.NameOfAccountHolder, "Please supply the name of the account holder");
                ctx.Required(m => m.NameOfBank, "Please supply the name of the bank");
                ctx.Custom(m => m.SortCode, sc => ValidateSortCode(bankDetails));
                ctx.Custom(m => m.AccountNumber, an => ValidateAccountNumber(bankDetails));
            }

            ctx.ThrowIfError();
        }

        private static string ValidateSortCode(BankDetails bankDetails)
        {
            var sc = bankDetails.SortCode;

            if (string.IsNullOrWhiteSpace(sc))
                return "Please supply the sort code";

            const string invalidMessage = "Please supply a valid Sort Code number in the format 'nn-nn-nn'";

            if (sc.Length != 8)
                return invalidMessage;

            if (sc[2] != '-' || sc[5] != '-')
                return invalidMessage;

            if (!AllCharsAreDigits(sc.Substring(0, 2)) || !AllCharsAreDigits(sc.Substring(3, 2)) || !AllCharsAreDigits(sc.Substring(6, 2)))
                return invalidMessage;

            return null;
        }

        private static string ValidateAccountNumber(BankDetails bankDetails)
        {
            var an = bankDetails.AccountNumber;

            if (string.IsNullOrWhiteSpace(an))
                return "Please supply the Account number";

            if (!AllCharsAreDigits(an))
                return "Please supply a valid Account number";

            return null;
        }

        private static bool AllCharsAreDigits(string value)
        {
            foreach (var c in value)
                if (!char.IsDigit(c))
                    return false;

            return true;
        }
    }
}