using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
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
            With(f => f.GuardianBenefits,           BenefitsBuilder.NewValid());
            With(f => f.GuardianPartnerBenefits,    BenefitsBuilder.NewValid());
            With(f => f.GuardianDetails,            GuardianDetailsBuilder.NewValid(Part.Part2));
            With(f => f.HealthProfessional,         HealthProfessionalBuilder.NewValid());
            With(f => f.PaymentDetails,             PaymentDetailsBuilder.NewValid());
            With(f => f.Declaration,                DeclarationBuilder.NewValid());
            return this;
        }
    }
}
