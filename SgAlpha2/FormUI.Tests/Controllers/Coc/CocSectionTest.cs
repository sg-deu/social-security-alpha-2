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

                Consent     = ConsentBuilder.NewValid(),
                Identity    = "test.email@example.com",
            };

            return detail;
        }
    }
}
