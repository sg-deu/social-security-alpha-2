using System;
using System.Collections.Generic;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public abstract class SectionStrategy
    {
        private static IDictionary<Sections, Func<SectionStrategy>> _strategies = new Dictionary<Sections, Func<SectionStrategy>>
        {
            { Sections.Consent,                 () => new SectionStrategy.Consent()             },
            { Sections.Identity,                () => new SectionStrategy.Identity()            },
            { Sections.Options,                 () => new SectionStrategy.Options()             },
            { Sections.ApplicantDetails,        () => new SectionStrategy.ApplicantDetails()    },
            { Sections.PaymentDetails,          () => new SectionStrategy.PaymentDetails()      },
            { Sections.Evidence,                () => new SectionStrategy.Evidence()            },
            { Sections.Declaration,             () => new SectionStrategy.Declaration()         },
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

        private class Options : SectionStrategy
        {
        }

        private class ApplicantDetails : SectionStrategy
        {
            public override bool Required(ChangeOfCircs form) { return Navigation.RequiresApplicantDetails(form); }
            public override void SkipSection(ChangeOfCircs form) { form.OnSkipApplicantDetails(); }
        }

        private class PaymentDetails : SectionStrategy
        {
            public override bool Required(ChangeOfCircs form) { return Navigation.RequiresPaymentDetails(form); }
            public override void SkipSection(ChangeOfCircs form) { form.OnSkipPaymentDetails(); }
        }

        private class Evidence : SectionStrategy
        {
        }

        private class Declaration : SectionStrategy
        {
        }
    }
}