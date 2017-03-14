using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Controllers.Bsg
{
    public abstract class BsgSectionTest : WebTest
    {
        protected static BsgDetail NewBsgDetail(string formId)
        {
            var detail = new BsgDetail
            {
                Id = formId,

                Consent                 = ConsentBuilder.NewValid(),
                ApplicantDetails        = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren        = ExpectedChildrenBuilder.NewValid(),
                ExistingChildren        = ExistingChildrenBuilder.NewValid(),
                ApplicantBenefits       = BenefitsBuilder.NewValid(),
                PartnerBenefits         = BenefitsBuilder.NewValid(),
                GuardianBenefits        = BenefitsBuilder.NewValid(),
                GuardianPartnerBenefits = BenefitsBuilder.NewValid(),
                PartnerDetails          = RelationDetailsBuilder.NewValid(Part.Part2),
                GuardianDetails         = RelationDetailsBuilder.NewValid(Part.Part2),
                GuardianPartnerDetails  = RelationDetailsBuilder.NewValid(Part.Part2),
                HealthProfessional      = HealthProfessionalBuilder.NewValid(),
                PaymentDetails          = PaymentDetailsBuilder.NewValid(),
                Declaration             = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
