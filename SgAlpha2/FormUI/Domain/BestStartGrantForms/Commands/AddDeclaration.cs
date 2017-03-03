using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddDeclaration : Command
    {
        public string       FormId;
        public Declaration  Declaration;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddDeclaration(Declaration);
        }
    }
}