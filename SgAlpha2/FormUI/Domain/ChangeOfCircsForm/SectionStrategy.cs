using System;
using System.Collections.Generic;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public abstract class SectionStrategy
    {
        private static IDictionary<Sections, Func<SectionStrategy>> _strategies = new Dictionary<Sections, Func<SectionStrategy>>
        {
            { Sections.Consent,                 () => new SectionStrategy.Consent()     },
            { Sections.Identity,                () => new SectionStrategy.Identity()    },
        };

        public static SectionStrategy For(Sections section)
        {
            if (!_strategies.ContainsKey(section))
                throw new Exception("Unhandled section: " + section);

            return _strategies[section]();
        }

        public virtual bool Required(ChangeOfCircs form)
        {
            return true; // default is to require sections
        }

        public virtual void SkipSection(ChangeOfCircs form)
        {
            throw new Exception("ClearSection not implemented for section: " + GetType());
        }

        private class Consent : SectionStrategy
        {
        }

        private class Identity : SectionStrategy
        {
        }
    }
}