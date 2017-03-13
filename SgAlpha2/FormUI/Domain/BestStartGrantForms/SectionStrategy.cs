using System;
using System.Collections.Generic;

namespace FormUI.Domain.BestStartGrantForms
{
    public abstract class SectionStrategy
    {
        private static IDictionary<Sections, Func<SectionStrategy>> _strategies = new Dictionary<Sections, Func<SectionStrategy>>
        {
            { Sections.Consent,                 () => new SectionStrategy.Consent() },
            { Sections.ApplicantDetails,        () => new SectionStrategy.ApplicantDetails() },
            { Sections.ExpectedChildren,        () => new SectionStrategy.ExpectedChildren() },
            { Sections.ExistingChildren,        () => new SectionStrategy.ExistingChildren() },
            { Sections.ApplicantBenefits,       () => new SectionStrategy.ApplicantBenefits() },
            { Sections.PartnerBenefits,         () => new SectionStrategy.PartnerBenefits() },
            { Sections.GuardianBenefits,        () => new SectionStrategy.GuardianBenefits() },
            { Sections.GuardianPartnerBenefits, () => new SectionStrategy.GuardianPartnerBenefits() },
            { Sections.GuardianDetails1,        () => new SectionStrategy.GuardianDetails1() },
            { Sections.GuardianDetails2,        () => new SectionStrategy.GuardianDetails2() },
            { Sections.HealthProfessional,      () => new SectionStrategy.HealthProfessional() },
            { Sections.PaymentDetails,          () => new SectionStrategy.PaymentDetails() },
            { Sections.Declaration,             () => new SectionStrategy.Declaration() },
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

        public virtual void SkipSection(BestStartGrant form)
        {
            throw new Exception("ClearSection not implemented for section: " + GetType());
        }

        private class Consent : SectionStrategy
        {
        }

        private class ApplicantDetails : SectionStrategy
        {
        }

        private class ExpectedChildren : SectionStrategy
        {
        }

        private class ExistingChildren : SectionStrategy
        {
        }

        private class ApplicantBenefits : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresApplicantBenefits(form); }
            public override void SkipSection(BestStartGrant form) { form.OnSkipApplicantBenefits(); }
        }

        private class PartnerBenefits : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresPartnerBenefits(form); }
            public override void SkipSection(BestStartGrant form) { form.OnSkipPartnerBenefits(); }
        }

        private class GuardianBenefits : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianBenefits(form); }
            public override void SkipSection(BestStartGrant form) { form.OnSkipGuardianBenefits(); }
        }

        private class GuardianPartnerBenefits : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianPartnerBenefits(form); }
            public override void SkipSection(BestStartGrant form) { form.OnSkipGuardianPartnerBenefits(); }
        }

        private class GuardianDetails1 : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianDetails(form); }
            public override void SkipSection(BestStartGrant form) { form.OnSkipGuardianDetails(); }
        }

        private class GuardianDetails2 : SectionStrategy
        {
            public override bool Required(BestStartGrant form) { return Navigation.RequiresGuardianDetails(form); }
            public override void SkipSection(BestStartGrant form) { /* already skipped */ }
        }

        private class HealthProfessional : SectionStrategy
        {
        }

        private class PaymentDetails : SectionStrategy
        {
        }

        private class Declaration : SectionStrategy
        {
        }
    }
}