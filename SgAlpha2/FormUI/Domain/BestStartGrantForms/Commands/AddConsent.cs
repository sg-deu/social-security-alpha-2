using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddConsent : Command
    {
        public string   FormId;
        public Consent  Consent;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddConsent(Consent);
        }
    }
}