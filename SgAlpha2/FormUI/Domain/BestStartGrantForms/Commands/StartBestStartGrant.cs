using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class StartBestStartGrant : Command<NextSection>
    {
        public override NextSection Execute()
        {
            return BestStartGrant.Start();
        }
    }
}