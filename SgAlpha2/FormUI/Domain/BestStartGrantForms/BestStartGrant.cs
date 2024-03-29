﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Consent              Consent                 { get; protected set; }
        public UKVerify             UKVerify                { get; protected set; }
        public ApplicantDetails     ApplicantDetails        { get; protected set; }
        public ExpectedChildren     ExpectedChildren        { get; protected set; }
        public ExistingChildren     ExistingChildren        { get; protected set; }
        public Benefits             ApplicantBenefits       { get; protected set; }
        public Benefits             PartnerBenefits         { get; protected set; }
        public Benefits             GuardianBenefits        { get; protected set; }
        public Benefits             GuardianPartnerBenefits { get; protected set; }
        public RelationDetails      GuardianDetails         { get; protected set; }
        public RelationDetails      PartnerDetails          { get; protected set; }
        public RelationDetails      GuardianPartnerDetails  { get; protected set; }
        public HealthProfessional   HealthProfessional      { get; protected set; }
        public PaymentDetails       PaymentDetails          { get; protected set; }
        public Evidence             Evidence                { get; protected set; }
        public Declaration          Declaration             { get; protected set; }

        private BsgDetail NewBsgDetail()
        {
            return new BsgDetail
            {
                Id                      = Id,

                Consent                 = Consent,
                UKVerify                = UKVerify,
                ApplicantDetails        = ApplicantDetails,
                ExpectedChildren        = ExpectedChildren,
                ExistingChildren        = ExistingChildren,
                ApplicantBenefits       = ApplicantBenefits,
                PartnerBenefits         = PartnerBenefits,
                GuardianBenefits        = GuardianBenefits,
                GuardianPartnerBenefits = GuardianPartnerBenefits,
                PartnerDetails          = PartnerDetails,
                GuardianDetails         = GuardianDetails,
                GuardianPartnerDetails  = GuardianPartnerDetails,
                HealthProfessional      = HealthProfessional,
                PaymentDetails          = PaymentDetails,
                Evidence                = Evidence,
                Declaration             = Declaration,
            };
        }

        public BsgDetail FindSection(Sections section)
        {
            var detail = NewBsgDetail();

            Navigation.Populate(detail, section, this);

            return detail;
        }

        public static NextSection Start()
        {
            var form = new BestStartGrant();
            Repository.Insert(form);

            return new NextSection
            {
                Id = form.Id,
                Type = NextType.Section,
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

        public static bool ShouldAskForNationalInsuranceNumber(ApplicantDetails applicantDetails)
        {
            var age = applicantDetails.Age();

            if (age < 16)
                return false;

            return true;
        }

        public static BsgDetail FindLatest(string userId)
        {
            var userForms = Repository.Query<BestStartGrant>()
                .Where(f => f.UserId == userId)
                .Where(f => f.Completed != null)
                .ToList();

            // when trying to the .OrderBy in the query,
            // it returns an empty list
            // https://github.com/Azure/azure-documentdb-dotnet/issues/65

            var latest = userForms
                .OrderByDescending(f => f.Completed.Value)
                .FirstOrDefault();

            return latest != null
                ? latest.NewBsgDetail()
                : null;
        }

        public NextSection AddConsent(Consent consent)
        {
            Validate(consent);

            Consent = consent;
            return OnSectionCompleted(Sections.Consent);
        }

        public NextSection AddUKVerify()
        {
            return OnSectionCompleted(Sections.UKVerify);
        }

        public NextSection AddApplicantDetails(ApplicantDetails applicantDetails)
        {
            Validate(applicantDetails);

            ApplicantDetails = applicantDetails;
            OnUserIdentified(applicantDetails.EmailAddress);
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

        public NextSection AddApplicantBenefits(Benefits applicantBenefits)
        {
            Validate(applicantBenefits);

            ApplicantBenefits = applicantBenefits;
            return OnSectionCompleted(Sections.ApplicantBenefits);
        }

        public NextSection AddPartnerBenefits(Benefits partnerBenefits)
        {
            Validate(partnerBenefits);

            PartnerBenefits = partnerBenefits;
            return OnSectionCompleted(Sections.PartnerBenefits);
        }

        public NextSection AddGuardianBenefits(Benefits guardianBenefits)
        {
            Validate(guardianBenefits);

            GuardianBenefits = guardianBenefits;
            return OnSectionCompleted(Sections.GuardianBenefits);
        }

        public NextSection AddGuardianPartnerBenefits(Benefits guardianPartnerBenefits)
        {
            Validate(guardianPartnerBenefits);

            GuardianPartnerBenefits = guardianPartnerBenefits;
            return OnSectionCompleted(Sections.GuardianPartnerBenefits);
        }

        public NextSection AddPartnerDetails(RelationDetails partnerDetails)
        {
            partnerDetails.RelationshipToApplicant = "Partner";

            Validate(partnerDetails);

            PartnerDetails = partnerDetails;
            return OnSectionCompleted(Sections.PartnerDetails);
        }

        public NextSection AddGuardianDetails(RelationDetails guardianDetails)
        {
            Validate(guardianDetails);

            GuardianDetails = guardianDetails;
            return OnSectionCompleted(Sections.GuardianDetails);
        }

        public NextSection AddGuardianPartnerDetails(RelationDetails guardianPartnerDetails)
        {
            Validate(guardianPartnerDetails);

            GuardianPartnerDetails = guardianPartnerDetails;
            return OnSectionCompleted(Sections.GuardianPartnerDetails);
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

        public void AddEvidenceFile(string filename, byte[] content)
        {
            var cloudName = Guid.NewGuid().ToString().ToLower() + Path.GetExtension(filename).ToLower();
            CloudStore.Store("bsg-" + Id, cloudName, filename, content);

            Evidence = Evidence ?? new Evidence();

            var file = new EvidenceFile
            {
                Name = filename,
                CloudName = cloudName,
            };

            Evidence.Files.Add(file);
            Repository.Update(this);
        }

        public void RemoveEvidenceFile(string cloudName)
        {
            Evidence = Evidence ?? new Evidence();

            int? index = null;

            for (int fileIndex = 0; fileIndex < Evidence.Files.Count; fileIndex++)
            {
                if (Evidence.Files[fileIndex].CloudName == cloudName)
                {
                    index = fileIndex;
                    cloudName = Evidence.Files[fileIndex].CloudName;
                }
            }

            if (index.HasValue)
            {
                CloudStore.Remove("bsg-" + Id, cloudName);

                Evidence.Files.RemoveAt(index.Value);
                Repository.Update(this);
            }
        }

        public NextSection AddEvidence(Evidence evidence)
        {
            Evidence = Evidence ?? evidence;
            evidence.CopyTo(Evidence);

            Validate(Evidence);

            return OnSectionCompleted(Sections.Evidence);
        }

        public NextSection AddDeclaration(Declaration declaration)
        {
            Validate(declaration);

            Declaration = declaration;
            return OnSectionCompleted(Sections.Declaration);
        }

        internal void OnSkipApplicantBenefits()         { ApplicantBenefits = null;         }
        internal void OnSkipPartnerBenefits()           { PartnerBenefits = null;           }
        internal void OnSkipGuardianBenefits()          { GuardianBenefits = null;          }
        internal void OnSkipGuardianPartnerBenefits()   { GuardianPartnerBenefits = null;   }
        internal void OnSkipPartnerDetails()            { PartnerDetails = null;            }
        internal void OnSkipGuardianDetails()           { GuardianDetails = null;           }
        internal void OnSkipGuardianPartnerDetails()    { GuardianPartnerDetails = null;    }

        private NextSection OnSectionCompleted(Sections section)
        {
            var next = Navigation.Next(this, section);

            var isFinalUpdate = next.Type == NextType.Complete;
            OnBeforeUpdate(isFinalUpdate);

            Repository.Update(this);
            return next;
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
            else
                applicantDetails.PreviouslyLookedAfter = null;

            if (ShouldAskEducationQuestion(applicantDetails))
                ctx.Required(m => m.FullTimeEducation, "Please indicate if you are 18/19 in full time education and part of your parents' or guardians' benefit claim");
            else
                applicantDetails.FullTimeEducation = null;

            if (ShouldAskForNationalInsuranceNumber(applicantDetails))
                ctx.Custom(m => m.NationalInsuranceNumber, ni => ValidateNationalInsuranceNumber(applicantDetails));
            else
                applicantDetails.NationalInsuranceNumber = null;

            ctx.Required(m => m.CurrentAddress.Line1, "Please supply an Address line 1");
            ctx.Required(m => m.CurrentAddress.Postcode, "Please supply a Postcode");

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

        internal static void Validate(ExpectedChildren expectedChildren)
        {
            var ctx = new ValidationContext<ExpectedChildren>(expectedChildren);

            ctx.Required(c => c.IsBabyExpected, "Please indicate if you are expecting a baby");

            if (expectedChildren.IsBabyExpected == false)
            {
                expectedChildren.ExpectancyDate = null;
                expectedChildren.IsMoreThan1BabyExpected = null;
                expectedChildren.ExpectedBabyCount = null;
            }

            if (expectedChildren.IsBabyExpected == true)
            {
                ctx.Required(m => m.ExpectancyDate, "Please supply the date the baby is due");

                if (expectedChildren.ExpectancyDate.HasValue)
                    ctx.InFuture(m => m.ExpectancyDate, "Please supply a due date in the future");

                ctx.Required(m => m.IsMoreThan1BabyExpected, "Please indicate if more than one baby is expected");

                if (expectedChildren.IsMoreThan1BabyExpected == false)
                    expectedChildren.ExpectedBabyCount = null;

                if (expectedChildren.IsMoreThan1BabyExpected == true)
                {
                    ctx.Required(m => m.ExpectedBabyCount, "Please supply how many babies are expected");

                    ctx.Custom(m => m.ExpectedBabyCount, babyCount =>
                        babyCount.HasValue && (babyCount.Value < 2 || babyCount.Value > 10)
                            ? "Please supply a number of babies between 2 and 10"
                            : null);
                }
            }

            ctx.ThrowIfError();
        }

        private static void Validate(ExistingChildren existingChildren)
        {
            var ctx = new ValidationContext<ExistingChildren>(existingChildren);

            ctx.Required(c => c.AnyExistingChildren, "Please indicate if you have children you're responsible for in your household");

            if (existingChildren.AnyExistingChildren == true && existingChildren.Children?.Count < 1)
                throw new DomainException("If you have children, please supply the details of at least one child");

            if (existingChildren.AnyExistingChildren == false && existingChildren.Children?.Count > 0)
                throw new DomainException("If you do not have children, please remove the supplied children");

            for (var i = 0; i < existingChildren.Children.Count; i ++)
            {
                ctx.Required(c => c.Children[i].FirstName, "Please supply a First name");
                ctx.Required(c => c.Children[i].Surname, "Please supply a Surname or family name");
                ctx.Required(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth");
                ctx.InPast(c => c.Children[i].DateOfBirth, "Please supply a Date of Birth in the past");
                ctx.Required(c => c.Children[i].Relationship, "Please supply the relationship to the child");
                ctx.Required(c => c.Children[i].ChildBenefit, "Please indicate if you receive child benefit");

                if (existingChildren.Children[i].ChildBenefit == false)
                    ctx.Required(c => c.Children[i].NoChildBenefitReason, "Please supply a reason why child benefit is not received");
            }

            ctx.ThrowIfError();
        }

        private static void Validate(Benefits benefits)
        {
            var ctx = new ValidationContext<Benefits>(benefits);

            if (benefits.HasExistingBenefit() == null)
                throw new DomainException("Please indicate if one of the stated benefits applies");

            if (benefits.HasExistingBenefit() == YesNoDk.Yes && benefits.None)
                throw new DomainException("Cannot select both \"None of the above\" and a benefit");

            if (benefits.HasExistingBenefit() == YesNoDk.Yes && benefits.Unknown)
                throw new DomainException("Cannot select both \"Don't know\" and a benefit");

            if (benefits.Unknown && benefits.None)
                throw new DomainException("Cannot select both \"Don't know\" and \"None of the above\"");

            ctx.ThrowIfError();
        }

        private static void Validate(RelationDetails relationDetails)
        {
            var ctx = new ValidationContext<RelationDetails>(relationDetails);

            ctx.Required(m => m.FullName, "Please supply a Full name");
            ctx.Required(m => m.DateOfBirth, "Please supply a Date of Birth");
            ctx.InPast(m => m.DateOfBirth, "Please supply a Date of Birth in the past");
            ctx.Custom(m => m.NationalInsuranceNumber, ni => ValidateNationalInsuranceNumber(relationDetails));
            ctx.Required(m => m.RelationshipToApplicant, "Please supply your Relationship to the applicant");

            if (!relationDetails.InheritAddress)
            {
                ctx.Required(m => m.Address.Line1, "Please supply an Address line 1");
                ctx.Required(m => m.Address.Postcode, "Please supply a Postcode");
            }

            ctx.ThrowIfError();
        }

        internal static void Validate(HealthProfessional healthProfessional)
        {
            var ctx = new ValidationContext<HealthProfessional>(healthProfessional);

            ctx.Required(c => c.Pin, "Please supply a GMC No. or NMC pin");

            ctx.ThrowIfError();
        }

        private static void Validate(Evidence evidence)
        {
            if (evidence.Files.Count == 0 && !evidence.SendingByPost)
                throw new DomainException("Please either upload evidence, or confirm that you are sending evidence by post");
        }

        private static void Validate(Declaration declaration)
        {
            var ctx = new ValidationContext<Declaration>(declaration);

            ctx.Required(m => m.AgreedToLegalStatement, "Please indicate that you agree");

            ctx.ThrowIfError();
        }
    }
}