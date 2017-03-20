using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Queries
{
    public class FindLatestApplication : Query<BsgDetail>
    {
        public string FormId;
        public string UserId;

        public override BsgDetail Find()
        {
            return BestStartGrant.FindLatest(UserId);
        }
    }
}