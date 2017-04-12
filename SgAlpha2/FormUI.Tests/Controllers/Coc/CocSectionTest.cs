using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Tests.Controllers.Util;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Controllers.Coc
{
    public abstract class CocSectionTest : WebTest
    {
        protected static CocDetail NewCocDetail(string formId)
        {
            var detail = new CocDetail
            {
                Id = formId,

                Consent             = ConsentBuilder.NewValid(),
                Identity            = "test.email@example.com",
                Options             = OptionsBuilder.NewValid(),
                ApplicantDetails    = ApplicantDetailsBuilder.NewValid(),
                ExpectedChildren    = ExpectedChildrenBuilder.NewValid(),
                HealthProfessional  = HealthProfessionalBuilder.NewValid(),
                PaymentDetails      = PaymentDetailsBuilder.NewValid(),
                Evidence            = EvidenceBuilder.NewValid(),
                Declaration         = DeclarationBuilder.NewValid(),
            };

            return detail;
        }
    }
}
