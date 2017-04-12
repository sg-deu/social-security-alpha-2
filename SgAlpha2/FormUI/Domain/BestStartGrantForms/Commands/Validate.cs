using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class Validate : Command<bool>
    {
        public ExpectedChildren     ExpectedChildren;
        public HealthProfessional   HealthProfessional;

        public override bool Execute()
        {
            if (ExpectedChildren != null)
                BestStartGrant.Validate(ExpectedChildren);

            if (HealthProfessional != null)
                BestStartGrant.Validate(HealthProfessional);

            return true;
        }
    }
}