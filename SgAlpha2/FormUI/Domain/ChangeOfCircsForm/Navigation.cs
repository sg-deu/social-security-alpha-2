using System;
using System.Collections.Generic;
using System.Linq;
using FormUI.Domain.ChangeOfCircsForm.Responses;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public static class Navigation
    {
        private static readonly IList<Sections> _order;

        static Navigation()
        {
            _order = Enum.GetValues(typeof(Sections)).Cast<Sections>().ToList();
        }

        public static IEnumerable<Sections> Order { get { return _order; } }

        public static void Populate(CocDetail detail, Sections section, ChangeOfCircs form)
        {
            var index = _order.IndexOf(section) - 1;

            while (index >= 0 && !detail.PreviousSection.HasValue)
            {
                var previousSection = _order[index];

                if (!FeatureToggles.SkipWorkInProgressSection(previousSection))
                {
                    var strategy = SectionStrategy.For(previousSection);

                    if (strategy.Required(form))
                        detail.PreviousSection = previousSection;
                }

                index--;
            }

            detail.IsFinalSection = (section == _order.Last());
        }

        public static NextSection Next(ChangeOfCircs form, Sections completedSection)
        {
            var index = _order.IndexOf(completedSection) + 1;

            Sections? nextSection = null;

            while (!nextSection.HasValue && index < _order.Count)
            {
                var section = _order[index];

                if (!FeatureToggles.SkipWorkInProgressSection(section))
                {
                    var strategy = SectionStrategy.For(section);

                    if (strategy.Required(form))
                        nextSection = section;
                    else
                        strategy.SkipSection(form);
                }

                index++;
            }

            return new NextSection
            {
                Id = form.Id,
                Section = nextSection,
            };
        }

        public static bool RequiresApplicantDetails(ChangeOfCircs form)
        {
            return form.Options?.ChangePersonalDetails == true;
        }
    }
}