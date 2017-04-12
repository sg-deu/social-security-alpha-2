using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
{
    public class HealthProfessionalBuilder
    {
        public static HealthProfessional NewValid(Action<HealthProfessional> mutator = null)
        {
            var healthProfessional = new HealthProfessional();
            BestStartGrantForms.Dto.HealthProfessionalBuilder.Populate(healthProfessional);
            mutator?.Invoke(healthProfessional);
            return healthProfessional;
        }
    }
}
