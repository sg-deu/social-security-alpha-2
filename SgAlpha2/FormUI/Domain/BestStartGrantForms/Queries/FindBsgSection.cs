using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Queries
{
    public class FindBsgSection : Query<BsgDetail>
    {
        public string FormId;

        public override BsgDetail Find()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.FindSection();
        }
    }
}