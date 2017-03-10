using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Domain
{
    public static class FeatureToggles
    {
        public static bool SkipWorkInProgressSection(Sections section)
        {
            return WorkingOnPartnerBenefits(section);
        }

        public static bool WorkingOnPartnerBenefits(Sections section)
        {
            return section == Sections.PartnerBenefits;
        }
    }
}