using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class BestStartGrantBuilder : Builder<BestStartGrant>
    {
        public BestStartGrantBuilder(string formId)
        {
            With(f => f.Id, formId);
        }

        public BestStartGrantBuilder WithCompletedSections(bool markAsCompleted = true)
        {
            With(f => f.Consent,                    ConsentBuilder.NewValid());
            With(f => f.ApplicantDetails,           ApplicantDetailsBuilder.NewValid());
            With(f => f.ExpectedChildren,           ExpectedChildrenBuilder.NewValid());
            With(f => f.ExistingChildren,           ExistingChildrenBuilder.NewValid());
            With(f => f.ApplicantBenefits,          BenefitsBuilder.NewWithBenefit());
            With(f => f.PartnerBenefits,            BenefitsBuilder.NewWithBenefit());
            With(f => f.GuardianBenefits,           BenefitsBuilder.NewWithBenefit());
            With(f => f.GuardianPartnerBenefits,    BenefitsBuilder.NewWithBenefit());
            With(f => f.PartnerDetails,             RelationDetailsBuilder.NewValid("partner"));
            With(f => f.GuardianDetails,            RelationDetailsBuilder.NewValid("guardian"));
            With(f => f.GuardianPartnerDetails,     RelationDetailsBuilder.NewValid("guardian partner"));
            With(f => f.HealthProfessional,         HealthProfessionalBuilder.NewValid());
            With(f => f.PaymentDetails,             PaymentDetailsBuilder.NewValid());
            With(f => f.Evidence,                   EvidenceBuilder.NewValid());
            With(f => f.Declaration,                DeclarationBuilder.NewValid());

            With(f => f.Started,                    DomainRegistry.NowUtc() - TimeSpan.FromHours(24));
            With(f => f.Completed,                  DomainRegistry.NowUtc());
            With(f => f.UserId,                     _instance.ApplicantDetails?.EmailAddress);
            VerifyConsistent(_instance);

            if (!markAsCompleted)
                With(f => f.Completed, null);

            return this;
        }

        public static void CopySectionsFrom(BestStartGrant form, BsgDetail detail)
        {
            detail.Id = form.Id;

            detail.Consent = form.Consent;
            detail.ApplicantDetails = form.ApplicantDetails;
            detail.ExpectedChildren = form.ExpectedChildren;
            detail.ExistingChildren = form.ExistingChildren;
            detail.ApplicantBenefits = form.ApplicantBenefits;
            detail.PartnerBenefits = form.PartnerBenefits;
            detail.GuardianBenefits = form.GuardianBenefits;
            detail.GuardianPartnerBenefits = form.GuardianPartnerBenefits;
            detail.PartnerDetails = form.PartnerDetails;
            detail.GuardianDetails = form.GuardianDetails;
            detail.GuardianPartnerDetails = form.GuardianPartnerDetails;
            detail.HealthProfessional = form.HealthProfessional;
            detail.PaymentDetails = form.PaymentDetails;
            detail.Evidence = form.Evidence;
            detail.Declaration = form.Declaration;
        }

        public static void VerifyConsistent(BestStartGrant doc)
        {
            if (!string.IsNullOrWhiteSpace(doc.ApplicantDetails?.EmailAddress))
                doc.UserId.Should().Be(doc.ApplicantDetails.EmailAddress, "Where an e-mail address has been supplied, the UserId should reflect this {0}", doc.Id);

            if (doc.Declaration?.AgreedToLegalStatement == true)
                doc.Completed.Should().NotBeNull("When consent has been filled in, expect a Completed date {0}", doc.Id);

            if (doc.PaymentDetails != null && doc.PaymentDetails.HasBankAccount == null)
                doc.PaymentDetails.HasBankAccount.Should().NotBeNull("When payment has been filled in, then HasBankAccount should be set {0}", doc.Id);

            if (doc.ExpectedChildren != null)
            {
                if (doc.ExpectedChildren.IsBabyExpected == null)
                    doc.ExpectedChildren.IsBabyExpected.Should().NotBeNull("When ExpectedChildren has been filled in, then IsBabyExpected should be set {0}", doc.Id);

                if (doc.ExpectedChildren.ExpectedBabyCount.HasValue)
                {
                    if (doc.ExpectedChildren.ExpectedBabyCount < 2)
                        doc.ExpectedChildren.ExpectedBabyCount.Value.Should().BeGreaterThan(1, "Expected baby count should be greater than 1 {0}", doc.Id);

                    doc.ExpectedChildren.IsMoreThan1BabyExpected.Should().HaveValue("When ExpectedBabyCount has been filled in, then IsMoreThan1BabyExpected should be set {0}", doc.Id);
                }
            }

            doc.Started.Should().NotBe(DateTime.MinValue);
        }

        public BestStartGrantBuilder PreviousApplicationFor(string userId)
        {
            WithCompletedSections();
            _instance.ApplicantDetails.EmailAddress = userId;
            With(f => f.UserId, userId);
            return this;
        }
    }
}
