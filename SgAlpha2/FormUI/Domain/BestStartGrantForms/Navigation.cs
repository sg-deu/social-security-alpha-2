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

        public static NextSection Next(BestStartGrant form, Sections completedSection)
        {
            if (IsIneligible(form, completedSection))
                return new NextSection
                {
                    Id = form.Id,
                    Type = NextType.Ineligible,
                    Section = null,
                };

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
                Type = nextSection.HasValue ? NextType.Section : NextType.Complete,
                Section = nextSection,
            };
        }

        public static bool IsIneligible(BestStartGrant form, Sections section)
        {
            if (section >= Sections.ExpectedChildren && section >= Sections.ExistingChildren)
                if (HasNoChildren(form))
                    return true;

            if (section >= Sections.ApplicantBenefits && section >= Sections.PartnerBenefits)
                if (HasNoQualifyingBenefits(form.ApplicantBenefits, form.PartnerBenefits))
                    return true;

            if (section >= Sections.GuardianBenefits && section >= Sections.GuardianPartnerBenefits)
                if (HasNoQualifyingBenefits(form.GuardianBenefits, form.GuardianPartnerBenefits))
                    return true;

            return false;
        }

        private static bool HasNoChildren(BestStartGrant form)
        {
            var expectedChildren = form.ExpectedChildren;
            var existingChildren = form.ExistingChildren;

            if (expectedChildren == null || existingChildren == null)
                return false; // can't know

            var hasExpectedChildren = expectedChildren.IsBabyExpected == true;
            var hasExistingChildren = existingChildren.Children.Count > 0;

            return !hasExpectedChildren && !hasExistingChildren;
        }

        private static bool HasNoQualifyingBenefits(Benefits mainBenefits, Benefits partnerBenefits)
        {
            if (mainBenefits == null || partnerBenefits == null)
                return false; // can't know

            return mainBenefits.HasExistingBenefit() == YesNoDk.No && partnerBenefits.HasExistingBenefit() == YesNoDk.No;
        }

        public static bool RequiresApplicantBenefits(BestStartGrant form)
        {
            if (BenefitsNotRequired(form))
                return false;

            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (applicantDetails.Age() < 18)
                    return false;

            if (RequiresGuardianBenefits(form))
                return false;

            return true;
        }

        public static bool RequiresPartnerBenefits(BestStartGrant form)
        {
            if (BenefitsNotRequired(form))
                return false;

            var applicantBenefits = form.ApplicantBenefits;

            if (applicantBenefits != null)
                if (applicantBenefits.HasExistingBenefit() != YesNoDk.Yes)
                    return true;

            return false;
        }

        public static bool RequiresGuardianBenefits(BestStartGrant form)
        {
            if (BenefitsNotRequired(form))
                return false;

            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (BestStartGrant.ShouldAskEducationQuestion(applicantDetails))
                    if (applicantDetails.FullTimeEducation == true)
                        return true;

            return false;
        }

        public static bool RequiresGuardianPartnerBenefits(BestStartGrant form)
        {
            if (BenefitsNotRequired(form))
                return false;

            var guardianBenefits = form.GuardianBenefits;

            if (guardianBenefits != null)
                if (guardianBenefits.HasExistingBenefit() != YesNoDk.Yes)
                    return true;

            return false;
        }

        public static bool RequiresPartnerDetails(BestStartGrant form)
        {
            var partnerBenefits = form.PartnerBenefits;

            if (RequiresPartnerBenefits(form) && partnerBenefits != null)
                if (partnerBenefits.HasExistingBenefit() != YesNoDk.No)
                    return true;

            return false;
        }

        public static bool RequiresGuardianDetails(BestStartGrant form)
        {
            var guardianBenefits = form.GuardianBenefits;

            if (RequiresGuardianBenefits(form) && guardianBenefits != null)
                if (guardianBenefits.HasExistingBenefit() != YesNoDk.No)
                    return true;

            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null)
                if (applicantDetails.Age() >= 16)
                    return false;

            return true;
        }

        public static bool RequiresGuardianPartnerDetails(BestStartGrant form)
        {
            var guardianPartnerBenefits = form.GuardianPartnerBenefits;

            if (RequiresGuardianPartnerBenefits(form) && guardianPartnerBenefits != null)
                if (guardianPartnerBenefits.HasExistingBenefit() != YesNoDk.No)
                    return true;

            return false;
        }

        private static bool BenefitsNotRequired(BestStartGrant form)
        {
            if (AllChildrenKinshipCare(form))
                return true;

            if (CareLeaver(form))
                return true;

            return false;
        }

        private static bool AllChildrenKinshipCare(BestStartGrant form)
        {
            var expectedChildren = form.ExpectedChildren;

            if (expectedChildren != null)
                if ((expectedChildren.ExpectedBabyCount.HasValue && expectedChildren.ExpectedBabyCount > 0) || expectedChildren.ExpectancyDate.HasValue)
                    return false;

            var existingChildren = form.ExistingChildren;

            if (existingChildren != null && existingChildren.Children != null)
                if (existingChildren.Children.Count > 0)
                    return existingChildren.Children.All(c => c.Relationship == Relationship.KinshipCarer);

            return false;
        }

        private static bool CareLeaver(BestStartGrant form)
        {
            var applicantDetails = form.ApplicantDetails;

            if (applicantDetails != null && applicantDetails.PreviouslyLookedAfter.HasValue)
                if (BestStartGrant.ShouldAskCareQuestion(applicantDetails))
                    if (applicantDetails.PreviouslyLookedAfter == true)
                        return true;

            return false;
        }
    }
}
