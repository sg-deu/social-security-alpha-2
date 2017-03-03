using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddExpectedChildren : Command<NextSection>
    {
        public string           FormId;
        public ExpectedChildren ExpectedChildren;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddExpectedChildren(ExpectedChildren);
        }
    }
}