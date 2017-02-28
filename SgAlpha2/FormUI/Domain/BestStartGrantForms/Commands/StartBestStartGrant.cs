using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class StartBestStartGrant : Command<string>
    {
        public Consent Consent;

        public override string Execute()
        {
            return BestStartGrant.Start(Consent);
        }
    }
}