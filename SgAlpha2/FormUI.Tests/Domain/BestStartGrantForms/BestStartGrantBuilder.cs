using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
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

        public BestStartGrantBuilder WithCompletedSections()
        {
            With(f => f.Consent,                    ConsentBuilder.NewValid());
            With(f => f.ApplicantDetails,           ApplicantDetailsBuilder.NewValid());
            With(f => f.ExpectedChildren,           ExpectedChildrenBuilder.NewValid());
            With(f => f.ExistingChildren,           ExistingChildrenBuilder.NewValid());
            With(f => f.ApplicantBenefits,          BenefitsBuilder.NewValid());
            With(f => f.PartnerBenefits,            BenefitsBuilder.NewValid());
            With(f => f.GuardianBenefits,           BenefitsBuilder.NewValid());
            With(f => f.GuardianPartnerBenefits,    BenefitsBuilder.NewValid());
            With(f => f.PartnerDetails,             RelationDetailsBuilder.NewValid(Part.Part2, "partner"));
            With(f => f.GuardianDetails,            RelationDetailsBuilder.NewValid(Part.Part2, "guardian"));
            With(f => f.GuardianPartnerDetails,     RelationDetailsBuilder.NewValid(Part.Part2, "guardian partner"));
            With(f => f.HealthProfessional,         HealthProfessionalBuilder.NewValid());
            With(f => f.PaymentDetails,             PaymentDetailsBuilder.NewValid());
            With(f => f.Declaration,                DeclarationBuilder.NewValid());

            With(f => f.Started, DomainRegistry.NowUtc() - TimeSpan.FromHours(24));
            With(f => f.Completed, DomainRegistry.NowUtc());
            With(f => f.UserId, _instance.ApplicantDetails?.EmailAddress);
            VerifyConsistent(_instance);
            return this;
        }

        public static void CopySectionsFrom(BestStartGrant form, BsgDetail detail)
        {
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
            detail.Declaration = form.Declaration;
        }

        public static void VerifyConsistent(BestStartGrant doc)
        {
            if (!string.IsNullOrWhiteSpace(doc.ApplicantDetails?.EmailAddress))
                doc.UserId.Should().Be(doc.ApplicantDetails.EmailAddress, "Where an e-mail address has been supplied, the UserId should reflect this {0}", doc.Id);

            if (doc.Declaration?.AgreedToLegalStatement == true)
                doc.Completed.Should().NotBeNull("When consent has been filled in, expect a Completed date {0}", doc.Id);

            doc.Started.Should().NotBe(DateTime.MinValue);
        }
    }
}
