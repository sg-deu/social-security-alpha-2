﻿using System;
using System.Collections.Generic;

namespace FormUI.Domain.BestStartGrantForms
{
    public abstract class SectionStrategy
    {
        private static IDictionary<Sections, Func<SectionStrategy>> _strategies = new Dictionary<Sections, Func<SectionStrategy>>
        {
            { Sections.Consent,                 () => new ConsentActions() },
            { Sections.ApplicantDetails,        () => new ApplicantDetailsActions() },
            { Sections.ExpectedChildren,        () => new ExpectedChildrenActions() },
            { Sections.ExistingChildren,        () => new ExistingChildrenActions() },
            { Sections.ApplicantBenefits,       () => new ApplicantBenefitsActions() },
            { Sections.GuardianBenefits,        () => new GuardianBenefitsActions() },
            { Sections.GuardianPartnerBenefits, () => new GuardianPartnerBenefitsActions() },
            { Sections.GuardianDetails1,        () => new GuardianDetails1Actions() },
            { Sections.GuardianDetails2,        () => new GuardianDetails2Actions() },
            { Sections.HealthProfessional,      () => new HealthProfessionalActions() },
            { Sections.PaymentDetails,          () => new PaymentDetailsActions() },
            { Sections.Declaration,             () => new DeclarationActions() },
        };

        public static SectionStrategy For(Sections section)
        {
            if (!_strategies.ContainsKey(section))
                throw new Exception("Unhandled section: " + section);

            return _strategies[section]();
        }

        public virtual bool Required(BestStartGrant form)
        {
            return true; // default is to require sections
        }

        private class ConsentActions : SectionStrategy
        {
        }

        private class ApplicantDetailsActions : SectionStrategy
        {
        }

        private class ExpectedChildrenActions : SectionStrategy
        {
        }

        private class ExistingChildrenActions : SectionStrategy
        {
        }

        private class ApplicantBenefitsActions : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresApplicantBenefits(form); }
        }

        private class GuardianBenefitsActions : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianBenefits(form); }
        }

        private class GuardianPartnerBenefitsActions : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianPartnerBenefits(form); }
        }

        private class GuardianDetails1Actions : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianDetails(form); }
        }

        private class GuardianDetails2Actions : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianDetails(form); }
        }

        private class HealthProfessionalActions : SectionStrategy
        {
        }

        private class PaymentDetailsActions : SectionStrategy
        {
        }

        private class DeclarationActions : SectionStrategy
        {
        }
    }
}