using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddDeclaration : Command<NextSection>
    {
        public string       FormId;
        public Declaration  Declaration;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddDeclaration(Declaration);
        }
    }
}