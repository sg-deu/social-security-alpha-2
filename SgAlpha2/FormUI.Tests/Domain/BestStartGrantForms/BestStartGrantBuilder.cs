using FormUI.Domain.BestStartGrantForms;
using FormUI.Tests.Domain.Util;
using System.Diagnostics.CodeAnalysis;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [ExcludeFromCodeCoverage]
    public class BestStartGrantBuilder : Builder<BestStartGrant>
    {
        public BestStartGrantBuilder(string formId)
        {
            With(f => f.Id, formId);
        }
    }
}
