using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddExistingChildren : Command
    {
        public string           FormId;
        public ExistingChildren ExistingChildren;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddExistingChildren(ExistingChildren);
        }
    }
}