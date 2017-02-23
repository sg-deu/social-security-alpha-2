using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddHealthProfessional : Command
    {
        public string               FormId;
        public HealthProfessional   HealthProfessional;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddHealthProfessional(HealthProfessional);
        }
    }
}