using System;
using System.Collections.Generic;
using FormUI.Domain.ChangeOfCircsForm;

namespace FormUI.Controllers.Coc
{
    public abstract class SectionActionStrategy
    {
        private static IDictionary<Sections, Func<SectionActionStrategy>> _strategies = new Dictionary<Sections, Func<SectionActionStrategy>>
        {
            { Sections.Consent,                 () => new ConsentActions()          },
            { Sections.Identity,                () => new IdentityActions()         },
            { Sections.Options,                 () => new OptionsActions()          },
            { Sections.ApplicantDetails,        () => new ApplicantDetailsActions() },
            { Sections.Evidence,                () => new EvidenceActions()         },
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
            public override string Action(string formId) { return CocActions.Consent(formId); }
        }

        private class IdentityActions : SectionActionStrategy
        {
            public override string Action(string formId) { return CocActions.Identity(formId); }
        }

        private class OptionsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return CocActions.Options(formId); }
        }

        private class ApplicantDetailsActions : SectionActionStrategy
        {
            public override string Action(string formId) { return CocActions.ApplicantDetails(formId); }
        }

        private class EvidenceActions : SectionActionStrategy
        {
            public override string Action(string formId) { return CocActions.Evidence(formId); }
        }
    }
}