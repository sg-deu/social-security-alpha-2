using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddHealthProfessional : Command<NextSection>
    {
        public string               FormId;
        public HealthProfessional   HealthProfessional;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddHealthProfessional(HealthProfessional);
        }
    }
}