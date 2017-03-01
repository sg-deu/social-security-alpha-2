using System;
using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Controllers.Bsg
{
    public abstract class SectionActionStrategy
    {
        public static SectionActionStrategy For(Sections section)
        {
            switch(section)
            {
                case Sections.Consent: return new ConsentActions();

                default:
                    throw new Exception("Unhandled section: " + section);
            }
        }

        public abstract string Action(string formId);

        private class ConsentActions : SectionActionStrategy
        {
            public override string Action(string formId) { return BsgActions.Consent(formId); }
        }
    }
}