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
                UKVerify                = UKVerifyBuilder.NewValid(),
                ApplicantDetails        = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren        = ExpectedChildrenBuilder.NewValid(),
                ExistingChildren        = ExistingChildrenBuilder.NewValid(),
                ApplicantBenefits       = BenefitsBuilder.NewWithBenefit(),
                PartnerBenefits         = BenefitsBuilder.NewWithBenefit(),
                GuardianBenefits        = BenefitsBuilder.NewWithBenefit(),
                GuardianPartnerBenefits = BenefitsBuilder.NewWithBenefit(),
                PartnerDetails          = RelationDetailsBuilder.NewValid(Part.Part2, "partner"),
                GuardianDetails         = RelationDetailsBuilder.NewValid(Part.Part2, "guardian"),
                GuardianPartnerDetails  = RelationDetailsBuilder.NewValid(Part.Part2, "guardian partner"),
                HealthProfessional      = HealthProfessionalBuilder.NewValid(),
                PaymentDetails          = PaymentDetailsBuilder.NewValid(),
                Declaration             = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
