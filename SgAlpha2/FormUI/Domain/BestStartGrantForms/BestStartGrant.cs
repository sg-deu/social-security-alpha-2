using System;
using System.Collections.Generic;
using System.Linq;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Forms;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms
{
    public class BestStartGrant : Form
    {
        protected BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public Consent              Consent             { get; protected set; }
        public ApplicantDetails     ApplicantDetails    { get; protected set; }
        public ExpectedChildren     ExpectedChildren    { get; protected set; }
        public ExistingChildren     ExistingChildren    { get; protected set; }
        public ApplicantBenefits    ApplicantBenefits   { get; protected set; }
        public GuardianDetails      GuardianDetails     { get; protected set; }
        public HealthProfessional   HealthProfessional  { get; protected set; }
        public PaymentDetails       PaymentDetails      { get; protected set; }
        public Declaration          Declaration         { get; protected set; }

        public BsgDetail FindSection(Sections section)
        {
            var detail = new BsgDetail
            {
                Consent             = Consent,
                ApplicantDetails    = ApplicantDetails,
                ExpectedChildren    = ExpectedChildren,
                ExistingChildren    = ExistingChildren,
                ApplicantBenefits   = ApplicantBenefits,
                GuardianDetails     = GuardianDetails,
                HealthProfessional  = HealthProfessional,
                PaymentDetails      = PaymentDetails,
                Declaration         = Declaration,
            };

            Navigation.Populate(detail, section);

            return detail;
        }

        public static NextSection Start()
        {
            var form = new BestStartGrant();
            Repository.Insert(form);

            return new NextSection
            {
                Id = form.Id,
                Section = Navigation.Order.First(),
            };
        }

        public static bool ShouldAskCareQuestion(ApplicantDetails applicantDetails)
        {
            var age = applicantDetails.Age();

            if (!age.HasValue)
                return false;

            return age >= 18 && age < 25;
        }

        public static bool ShouldAskEducationQuestion(ApplicantDetails applicantDetails)
        {
            var age = applicantDetails.Age();

            if (!age.HasValue)
                return false;

            return age == 18 || age == 19;
        }

        public NextSection AddConsent(Consent consent)
        {
            Validate(consent);

            Consent = consent;
            return OnSectionCompleted(Sections.Consent);
        }

        public NextSection AddApplicantDetails(ApplicantDetails applicantDetails)
        {
            Validate(applicantDetails);

            ApplicantDetails = applicantDetails;
            return OnSectionCompleted(Sections.ApplicantDetails);
        }

        public NextSection AddExpectedChildren(ExpectedChildren expectedChildren)
        {
            Validate(expectedChildren);

            ExpectedChildren = expectedChildren;
            return OnSectionCompleted(Sections.ExpectedChildren);
        }

        public NextSection AddExistingChildren(ExistingChildren existingChildren)
        {
            Validate(existingChildren);

            ExistingChildren = existingChildren;
            return OnSectionCompleted(Sections.ExistingChildren);
        }

        public NextSection AddApplicantBenefits(Part part, ApplicantBenefits applicantBenefits)
        {
            Validate(part, applicantBenefits);

            ApplicantBenefits = ApplicantBenefits ?? new ApplicantBenefits();
            applicantBenefits.CopyTo(ApplicantBenefits, part);

            var section = part == Part.Part1
                ? Sections.ApplicantBenefits1
                : Sections.ApplicantBenefits2;

            return OnSectionCompleted(section);
        }

        public NextSection AddGuardianDetails(Part part, GuardianDetails guardianDetails)
        {
            Validate(part, guardianDetails);

            GuardianDetails = GuardianDetails ?? new GuardianDetails();
            guardianDetails.CopyTo(GuardianDetails, part);

            var section = part == Part.Part1
                ? Sections.GuardianDetails1
                : Sections.GuardianDetails2;

            return OnSectionCompleted(section);
        }

        public NextSection AddHealthProfessional(HealthProfessional healthProfessional)
        {
            Validate(healthProfessional);

            HealthProfessional = healthProfessional;
            return OnSectionCompleted(Sections.HealthProfessional);
        }

        public NextSection AddPaymentDetails(PaymentDetails paymentDetails)
        {
            Validate(paymentDetails);

            PaymentDetails = paymentDetails;
            return OnSectionCompleted(Sections.PaymentDetails);
        }

        public NextSection AddDeclaration(Declaration declaration)
        {
            Validate(declaration);

            Declaration = declaration;
            return OnSectionCompleted(Sections.Declaration);
        }

        private NextSection OnSectionCompleted(Sections section)
        {
            Repository.Update(this);
            return Navigation.Next(this, section);
        }

        private static void Validate(Consent consent)
        {
            var ctx = new ValidationContext<Consent>(consent);

            ctx.Required(m => m.AgreedToConsent, "Please indicate that you agree");

            ctx.ThrowIfError();
        }

        private static void Validate(ApplicantDetails applicantDetails)
        {
            var ctx = new ValidationContext<ApplicantDetails>(applicantDetails);

            ctx.Required(m => m.FirstName, "Please supply a First name");
            ctx.Required(m => m.SurnameOrFamilyName, "Please supply a Surname or family name");
            ctx.Required(m => m.DateOfBirth, "Please supply a Date of Birth");
            ctx.InPast(m => m.DateOfBirth, "Please supply a Date of Birth in the past");

            if (ShouldAskCareQuestion(applicantDetails))
                ctx.Required(m => m.PreviouslyLookedAfter, "Please indicate if you have previously been looked after");

            if (ShouldAskEducationQuestion(applicantDetails))
                ctx.Required(m => m.FullTimeEducation, "Please indicate if you are 18/19 in full time education and part of your parents' or guardians' benefit claim");

            ctx.Custom(m => m.NationalInsuranceNumber, ni => ValidateNationalInsuranceNumber(applicantDetails));
            ctx.Required(m => m.CurrentAddress.Line1, "Please supply an Address line 1");
            ctx.Required(m => m.CurrentAddress.Line2, "Please supply an Address line 2");
            ctx.Required(m => m.CurrentAddress.Postcode, "Please supply a Postcode");
            ctx.Required(m => m.DateMovedIn, "Please supply the Date You or your Partner moved into this address");
            ctx.Required(m => m.CurrentAddressStatus, "Please indicate if this address is Permanent or Temporary");
            ctx.Required(m => m.ContactPreference, "Please supply a contact preference");

            if (applicantDetails.ContactPreference.HasValue)
                switch(applicantDetails.ContactPreference.Value)
                {
                    case ContactPreference.Email:
                        ctx.Required(m => m.EmailAddress, "Please supply an Email address");
                        break;

                    case ContactPreference.Phone:
                        ctx.Required(m => m.PhoneNumer, "Please supply a Phone number");
                        break;

                    case ContactPreference.Text:
                        ctx.Required(m => m.MobilePhoneNumber, "Please supply a Mobile phone number");
                        break;

                    default:
                        throw new Exception("Unhandled contact preference: " + applicantDetails.ContactPreference);
                }

            ctx.ThrowIfError();
        }

        private static string ValidateNationalInsuranceNumber(INationalInsuranceNumberHolder holder)
        {
            var ni = holder.NationalInsuranceNumber;

            if (string.IsNullOrWhiteSpace(ni))
                return "Please supply a National Insurance number";

            ni = ni.Replace(" ", "").ToUpper();

            // if ni is "AB/123456/C"
            if (ni.Length > 2 && ni[2] == '/')
                ni = ni.Substring(0, 2) + ni.Substring(3, ni.Length - 3);

            // if ni is "AB123456/C"
            if (ni.Length > 8 && ni[8] == '/')
                ni = ni.Substring(0, 8) + ni.Substring(9, ni.Length - 9);

            const string invalidMessage = "Please supply a valid National Insurance number in the format 'AB 12 34 56 C'";

            if (ni.Length != 9)
                return invalidMessage;

            // ni should now be "AB123456C", so positions 0, 1, and 8 are letters, and the rest are numbers
            var letterPositions = new List<int> { 0, 1, 8 };
            for (var characterIndex = 0; characterIndex < ni.Length; characterIndex++)
            {
                if (letterPositions.Contains(characterIndex) && !char.IsLetter(ni[characterIndex]))
                    return invalidMessage;

                if (!letterPositions.Contains(characterIndex) && !char.IsNumber(ni[characterIndex]))
                    return invalidMessage;
            }

            // now we know it's valid, put spaces (back) in to format correctly
            ni = string.Format("{0} {1} {2} {3} {4}",
                ni.Substring(0, 2),     // {0} AB
                ni.Substring(2, 2),     // {1} 12
                ni.Substring(4, 2),     // {2} 34
                ni.Substring(6, 2),     // {3} 56
                ni.Substring(8, 1));    // {4} C

            holder.NationalInsuranceNumber = ni;

            return null;
        }

        private static void Validate(ExpectedChildren expectedChildren)
        {
            var ctx = new ValidationContext<ExpectedChildren>(expectedChildren);

            ctx.InFuture(m => m.ExpectancyDate, "Please supply an expectancy date in the future");

            ctx.Custom(m => m.ExpectedBabyCount, babyCount =>
                babyCount.HasValue && (babyCount.Value < 1 || babyCount.Value > 10)
                    ? "Please supply a number of babies between 1 and 10"
                    : null);

            ctx.ThrowIfError();
        }

        private static void Validate(ExistingChildren existingChildren)
        {
            var ctx = new ValidationContext<ExistingChildren>(existingChildren);

            for (var i = 0; i < existingChildren.Children.Count; i ++)
            {
                ctx.Required(c => c.Children[i].FirstName, "Please supply a First name");
                ctx.Required(c => c.Children[i].Surname, "Please supply a Surname or family name");
                ctx.Required(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth");
                ctx.InPast(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth in the past");
                ctx.Required(c => c.Children[i].RelationshipToChild, "Please supply the relationship to the child");
                ctx.Required(c => c.Children[i].FormalKinshipCare, "Please indicate is their is formal kinship care");
            }

            ctx.ThrowIfError();
        }

        private static void Validate(Part part, ApplicantBenefits applicantBenefits)
        {
            var ctx = new ValidationContext<ApplicantBenefits>(applicantBenefits);

            if (part == Part.Part1)
            {
                ctx.Required(b => b.HasExistingBenefit, "Please indicate if you have one of the stated benefits");
            }

            if (part == Part.Part2)
            {
                ctx.Required(b => b.ReceivingBenefitForUnder20, "Please indicate if you are receiving benefit for the parent of the baby, or an expectant mother, because they are under 20 years of age");
                ctx.Required(b => b.YouOrPartnerInvolvedInTradeDispute, "Please indicate if you or your partner are involved in a trade dispute");
            }

            ctx.ThrowIfError();
        }

        private static void Validate(Part part, GuardianDetails guardianDetails)
        {
            var ctx = new ValidationContext<GuardianDetails>(guardianDetails);

            if (part == Part.Part1)
            {
                ctx.Required(m => m.FullName, "Please supply a Full name");
                ctx.Required(m => m.DateOfBirth, "Please supply a Date of Birth");
                ctx.InPast(m => m.DateOfBirth, "Please supply a Date of Birth in the past");
                ctx.Custom(m => m.NationalInsuranceNumber, ni => ValidateNationalInsuranceNumber(guardianDetails));
                ctx.Required(m => m.RelationshipToApplicant, "Please supply your Relationship to the applicant");
            }

            if (part == Part.Part2)
            {
                ctx.Required(m => m.Address.Line1, "Please supply an Address line 1");
                ctx.Required(m => m.Address.Line2, "Please supply an Address line 2");
                ctx.Required(m => m.Address.Postcode, "Please supply a Postcode");
            }

            ctx.ThrowIfError();
        }

        private static void Validate(HealthProfessional healthProfessional)
        {
            var ctx = new ValidationContext<HealthProfessional>(healthProfessional);

            ctx.Required(c => c.Pin, "Please supply a GMC No. or NMC pin");

            ctx.ThrowIfError();
        }

        private static void Validate(PaymentDetails paymentDetails)
        {
            var ctx = new ValidationContext<PaymentDetails>(paymentDetails);

            ctx.Required(m => m.LackingBankAccount, "Please indicate if you are lacking a bank account");

            if (paymentDetails.LackingBankAccount == false)
            {
                ctx.Required(m => m.NameOfAccountHolder, "Please supply the name of the account holder");
                ctx.Required(m => m.NameOfBank, "Please supply the name of the bank");
                ctx.Custom(m => m.SortCode, sc => ValidateSortCode(paymentDetails));
                ctx.Custom(m => m.AccountNumber, an => ValidateAccountNumber(paymentDetails));
            }

            ctx.ThrowIfError();
        }

        private static void Validate(Declaration declaration)
        {
            var ctx = new ValidationContext<Declaration>(declaration);

            ctx.Required(m => m.AgreedToLegalStatement, "Please indicate that you agree");

            ctx.ThrowIfError();
        }

        private static string ValidateSortCode(PaymentDetails paymentDetails)
        {
            var sc = paymentDetails.SortCode;

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

        private static string ValidateAccountNumber(PaymentDetails paymentDetails)
        {
            var an = paymentDetails.AccountNumber;

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