using FormUI.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    public class BestStartGrantBuilder : Builder<BestStartGrant>
    {
        public BestStartGrantBuilder(string formId)
        {
            With(f => f.Id, formId);
        }
    }
}
