using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddIdentity : Command<NextSection>
    {
        public string FormId;
        public string Identity;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddIdentity(Identity);
        }
    }
}