using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    public class ChangeOfCircsBuilder : Builder<ChangeOfCircs>
    {
        public ChangeOfCircsBuilder(string formId)
        {
            With(f => f.Id, formId);
        }

        public ChangeOfCircsBuilder WithCompletedSections()
        {
            With(f => f.Consent, ConsentBuilder.NewValid());
            return this;
        }

        public static void CopySectionsFrom(ChangeOfCircs form, CocDetail detail)
        {
            detail.Consent = form.Consent;
        }
    }
}
