using System;
using System.IO;
using System.Linq;
using System.Text;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Forms;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public class ChangeOfCircs : Form
    {
        protected ChangeOfCircs() : base(Guid.NewGuid().ToString())
        {
        }

        public Consent              Consent                     { get; protected set; }
        public ApplicantDetails     ExistingApplicantDetails    { get; protected set; }
        public PaymentDetails       ExistingPaymentDetails      { get; protected set; }
        public Options              Options                     { get; protected set; }
        public ApplicantDetails     ApplicantDetails            { get; protected set; }
        public ExpectedChildren     ExpectedChildren            { get; protected set; }
        public HealthProfessional   HealthProfessional          { get; protected set; }
        public PaymentDetails       PaymentDetails              { get; protected set; }
        public Evidence             Evidence                    { get; protected set; }
        public Declaration          Declaration                 { get; protected set; }

        public CocDetail FindSection(Sections section)
        {
            var detail = new CocDetail
            {
                Consent             = Consent,
                Identity            = UserId,
                Options             = Options,
                ApplicantDetails    = ApplicantDetails ?? ExistingApplicantDetails,
                ExpectedChildren    = ExpectedChildren,
                HealthProfessional  = HealthProfessional,
                PaymentDetails      = PaymentDetails ?? ExistingPaymentDetails,
                Evidence            = Evidence,
                Declaration         = Declaration,
            };

            Navigation.Populate(detail, section, this);

            return detail;
        }

        public static NextSection Start()
        {
            var form = new ChangeOfCircs();
            Repository.Insert(form);

            return new NextSection
            {
                Id = form.Id,
                Section = Navigation.Order.First(),
            };
        }

        public NextSection AddConsent(Consent consent)
        {
            Validate(consent);

            Consent = consent;
            return OnSectionCompleted(Sections.Consent);
        }

        public NextSection AddIdentity(string userId)
        {
            ValidateIdentity(userId);

            var existingForm = new FindLatestApplication { UserId = userId }.Find();

            if (existingForm == null)
                throw new DomainException("Could not find any existing application for the supplied email");

            UserId = userId;

            var fullName = new StringBuilder();
            fullName.Append(existingForm.ApplicantDetails.FirstName);

            if (!string.IsNullOrEmpty(existingForm.ApplicantDetails.OtherNames))
                fullName.AppendFormat(" {0}", existingForm.ApplicantDetails.OtherNames);

            fullName.AppendFormat(" {0}", existingForm.ApplicantDetails.SurnameOrFamilyName);

            ExistingApplicantDetails = new ApplicantDetails
            {
                Title = existingForm.ApplicantDetails.Title,
                FullName = fullName.ToString(),
                Address = existingForm.ApplicantDetails.CurrentAddress,
                MobilePhoneNumber = existingForm.ApplicantDetails.MobilePhoneNumber,
                HomePhoneNumer = existingForm.ApplicantDetails.PhoneNumer,
                EmailAddress = existingForm.ApplicantDetails.EmailAddress,
                Age = existingForm.ApplicantDetails.Age(),
            };

            ExistingPaymentDetails = new PaymentDetails
            {
                HasBankAccount = existingForm.PaymentDetails.HasBankAccount,
            };

            return OnSectionCompleted(Sections.Identity);
        }

        public NextSection AddOptions(Options options)
        {
            Validate(options);

            Options = options;
            return OnSectionCompleted(Sections.Options);
        }

        public NextSection AddApplicantDetails(ApplicantDetails applicantDetails)
        {
            Validate(applicantDetails);

            ApplicantDetails = applicantDetails;
            return OnSectionCompleted(Sections.ApplicantDetails);
        }

        public NextSection AddExpectedChildren(ExpectedChildren expectedChildren)
        {
            if (expectedChildren.IsBabyExpected == false)
                throw new DomainException("Must indicate having a baby in Change of Circumstances");

            BestStartGrantForms.BestStartGrant.Validate(expectedChildren);

            ExpectedChildren = expectedChildren;
            return OnSectionCompleted(Sections.ExpectedChildren);
        }

        public NextSection AddHealthProfessional(HealthProfessional healthProfessional)
        {
            BestStartGrantForms.BestStartGrant.Validate(healthProfessional);

            HealthProfessional = healthProfessional;
            return OnSectionCompleted(Sections.HealthProfessional);
        }

        public NextSection AddPaymentDetails(PaymentDetails paymentDetails)
        {
            Validate(paymentDetails);

            if (ExistingPaymentDetails.HasBankAccount == false && paymentDetails.HasBankAccount == false)
                throw new DomainException("Please indicate your new bank account details");

            PaymentDetails = paymentDetails;
            return OnSectionCompleted(Sections.PaymentDetails);
        }

        public void AddEvidenceFile(string filename, byte[] content)
        {
            var cloudName = Guid.NewGuid().ToString().ToLower() + Path.GetExtension(filename).ToLower();
            CloudStore.Store("coc-" + Id, cloudName, filename, content);

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
                }
            }

            if (index.HasValue)
            {
                CloudStore.Remove("coc-" + Id, cloudName);

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

        internal void OnSkipApplicantDetails()      { ApplicantDetails = null;      }
        internal void OnSkipExpectedChildren()      { ExpectedChildren = null;      }
        internal void OnSkipHealthProfessional()    { HealthProfessional = null;    }
        internal void OnSkipPaymentDetails()        { PaymentDetails = null;        }

        private NextSection OnSectionCompleted(Sections section)
        {
            var next = Navigation.Next(this, section);

            var isFinalUpdate = !next.Section.HasValue;
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

        private static void ValidateIdentity(string userId)
        {
            var ctx = new ValidationContext<string>(userId);

            ctx.Required(m => m, "Please supply your e-mail address");

            ctx.ThrowIfError();
        }

        private static void Validate(Options options)
        {
            var anyChanged = options.ChangePersonalDetails
                || options.ChangePartnerDetails
                || options.ChangeChildrenDetails
                || options.ChangePaymentDetails
                || options.Other;

            if (!anyChanged)
                throw new DomainException("Please select at least one change");

            var ctx = new ValidationContext<Options>(options);

            {
                // until these are implemented, they cannot be supplied as a change
                ctx.Custom(m => m.ChangePartnerDetails, c => c ? "Cannot currently change partner's details" : null);
                ctx.Custom(m => m.ChangeChildrenDetails, c => c ? "Cannot currently change children's details" : null);
            }

            if (options.Other)
                ctx.Required(m => m.OtherDetails, "Please supply the details of the change");

            ctx.ThrowIfError();
        }

        private static void Validate(ApplicantDetails applicantDetails)
        {
            var ctx = new ValidationContext<ApplicantDetails>(applicantDetails);

            ctx.Required(m => m.FullName, "Please supply a Full name");
            ctx.Required(m => m.Address.Line1, "Please supply an Address line 1");
            ctx.Required(m => m.Address.Postcode, "Please supply a Postcode");

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