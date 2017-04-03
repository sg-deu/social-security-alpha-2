using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Controllers.Bsg
{
    public abstract class BsgSectionTest : WebTest
    {
        protected static BsgDetail NewBsgDetail(string formId, int childCount = 2)
        {
            var detail = new BsgDetail
            {
                Id = formId,

                Consent                 = ConsentBuilder.NewValid(),
                UKVerify                = UKVerifyBuilder.NewValid(),
                ApplicantDetails        = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren        = ExpectedChildrenBuilder.NewValid(),
                ExistingChildren        = ExistingChildrenBuilder.NewValid(childCount),
                ApplicantBenefits       = BenefitsBuilder.NewWithBenefit(),
                PartnerBenefits         = BenefitsBuilder.NewWithBenefit(),
                GuardianBenefits        = BenefitsBuilder.NewWithBenefit(),
                GuardianPartnerBenefits = BenefitsBuilder.NewWithBenefit(),
                PartnerDetails          = RelationDetailsBuilder.NewValid("partner"),
                GuardianDetails         = RelationDetailsBuilder.NewValid("guardian"),
                GuardianPartnerDetails  = RelationDetailsBuilder.NewValid("guardian partner"),
                HealthProfessional      = HealthProfessionalBuilder.NewValid(),
                PaymentDetails          = PaymentDetailsBuilder.NewValid(),
                Declaration             = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
