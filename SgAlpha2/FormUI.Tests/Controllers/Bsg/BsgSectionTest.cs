using FormUI.Domain.BestStartGrantForms.Dto;
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
                Evidence                = EvidenceBuilder.NewValid(),
                Declaration             = DeclarationBuilder.NewValid(),
            };

            return detail;
        }

        protected static BsgDetail NewMinimalBsgDetail(string formId)
        {
            var detail = new BsgDetail
            {
                Id = formId,

                Consent                 = new Consent(),
                ApplicantDetails        = new ApplicantDetails(),
                ExpectedChildren        = new ExpectedChildren(),
                ExistingChildren        = new ExistingChildren(),
                ApplicantBenefits       = new Benefits(),
                PartnerBenefits         = new Benefits(),
                GuardianBenefits        = new Benefits(),
                GuardianPartnerBenefits = new Benefits(),
                PartnerDetails          = new RelationDetails(),
                GuardianDetails         = new RelationDetails(),
                GuardianPartnerDetails  = new RelationDetails(),
                HealthProfessional      = new HealthProfessional(),
                PaymentDetails          = new PaymentDetails(),
                Declaration             = new Declaration(),
            };

            return detail;
        }
    }
}
