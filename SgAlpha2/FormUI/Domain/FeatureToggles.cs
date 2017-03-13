using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Domain
{
    public static class FeatureToggles
    {
        public static bool SkipWorkInProgressSection(Sections section)
        {
            return WorkingOnGuardianPartnerDetails(section);
        }

        public static bool WorkingOnGuardianPartnerDetails(Sections section)
        {
            return section == Sections.GuardianPartnerDetails1 || section == Sections.GuardianPartnerDetails2;
        }
    }
}