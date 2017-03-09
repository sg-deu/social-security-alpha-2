using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Controllers.Bsg
{
    public abstract class SectionActionStrategy
    {
        private static IDictionary<Sections, Func<SectionActionStrategy>> _strategies = new Dictionary<Sections, Func<SectionActionStrategy>>
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

        public static SectionActionStrategy For(Sections section)
        {
            if (!_strategies.ContainsKey(section))
                throw new Exception("Unhandled section: " + section);

            return _strategies[section]();
        }

        public abstract string Action(string formId);

        private class ConsentActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.Consent(formId); }
        }

        private class ApplicantDetailsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.ApplicantDetails(formId); }
        }

        private class ExpectedChildrenActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.ExpectedChildren(formId); }
        }

        private class ExistingChildrenActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.ExistingChildren(formId); }
        }

        private class ApplicantBenefitsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.ApplicantBenefits(formId); }
        }

        private class GuardianBenefitsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.GuardianBenefits(formId); }
        }

        private class GuardianPartnerBenefitsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.GuardianPartnerBenefits(formId); }
        }

        private class GuardianDetails1Actions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.GuardianDetails1(formId); }
        }

        private class GuardianDetails2Actions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.GuardianDetails2(formId); }
        }

        private class HealthProfessionalActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.HealthProfessional(formId); }
        }

        private class PaymentDetailsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.PaymentDetails(formId); }
        }

        private class DeclarationActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.Declaration(formId); }
        }
    }
}