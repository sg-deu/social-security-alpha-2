﻿using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class HealthProfessionalBuilder
    {
        public static HealthProfessional NewValid(Action<HealthProfessional> mutator = null)
        {
            return Populate(new HealthProfessional(), mutator);
        }

        public static HealthProfessional Populate(HealthProfessional healthProfessional, Action<HealthProfessional> mutator = null)
        {
            healthProfessional.Pin = "GMC12345";

            mutator?.Invoke(healthProfessional);

            return healthProfessional;
        }

        public static HealthProfessional Invalid(this HealthProfessional healthProfessional)
        {
            healthProfessional.Pin = null;
            return healthProfessional;
        }
    }
}
