using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddConsent : Command<NextSection>
    {
        public string   FormId;
        public Consent  Consent;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddConsent(Consent);
        }
    }
}