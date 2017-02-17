using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class StartBestStartGrant : Command
    {
        public AboutYou AboutYou;

        public override void Execute()
        {
            BestStartGrant.Start(AboutYou);
        }
    }
}