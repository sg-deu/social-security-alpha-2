using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class StartBestStartGrant : Command<string>
    {
        public AboutYou AboutYou;

        public override string Execute()
        {
            return BestStartGrant.Start(AboutYou);
        }
    }
}