using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Domain
{
    public static class FeatureToggles
    {
        public static bool SkipWorkInProgressSection(Sections section)
        {
            return WorkingOnPartnerDetails(section);
        }

        public static bool WorkingOnPartnerDetails(Sections section)
        {
            return section == Sections.PartnerDetails1 || section == Sections.PartnerDetails2;
        }
    }
}