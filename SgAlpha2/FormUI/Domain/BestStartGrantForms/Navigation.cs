using System;
using System.Collections.Generic;
using System.Linq;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;

namespace FormUI.Domain.BestStartGrantForms
{
    public static class Navigation
    {
        private static readonly IList<Sections> _order;

        static Navigation()
        {
            _order = Enum.GetValues(typeof(Sections)).Cast<Sections>().ToList();
        }

        public static IEnumerable<Sections> Order { get { return _order; } }

        public static void Populate(BsgDetail detail, Sections section, BestStartGrant form)
        {
            var index = _order.IndexOf(section) - 1;

            while (index >= 0 && !detail.PreviousSection.HasValue)
            {
                var previousSection = _order[index];
                var strategy = SectionStrategy.For(previousSection);

                if (strategy.Required(form))
                    detail.PreviousSection = previousSection;

                index--;
            }

            detail.IsFinalSection = index == _order.Count - 1;
        }

        public static NextSection Next(BestStartGrant form, Sections completedSection)
        {
            var index = _order.IndexOf(completedSection) + 1;

            Sections? nextSection = null;

            while (!nextSection.HasValue && index < _order.Count)
            {
                var section = _order[index];
                var strategy = SectionStrategy.For(section);

                if (strategy.Required(form))
                    nextSection = section;

                index++;
            }

            return new NextSection
            {
                Id = form.Id,
                Section = nextSection,
            };
        }

        public static bool RequiresApplicantBenefits(BestStartGrant form)
        {
            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (applicantDetails.Age() < 18)
                    return false;

            return true;
        }

        public static bool RequiresGuardianDetails(BestStartGrant form)
        {
            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (applicantDetails.Age() >= 16)
                    return false;

            return true;
        }

        public static bool RequiresGuardianBenefits(BestStartGrant form)
        {
            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (BestStartGrant.ShouldAskEducationQuestion(applicantDetails))
                    if (applicantDetails.FullTimeEducation == true)
                        return true;

            return false;
        }

        public static bool RequiresGuardianPartnerBenefits(BestStartGrant form)
        {
            var guardianBenefits = form.GuardianBenefits;

            if (guardianBenefits != null)
                if (guardianBenefits.HasExistingBenefit != YesNoDk.Yes)
                    return true;

            return false;
        }
    }
}
