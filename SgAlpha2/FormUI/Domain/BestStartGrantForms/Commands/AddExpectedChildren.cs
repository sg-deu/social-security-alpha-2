using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddExpectedChildren : Command
    {
        public string           FormId;
        public ExpectedChildren ExpectedChildren;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddExpectedChildren(ExpectedChildren);
        }
    }
}