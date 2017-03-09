using FormUI.Domain.BestStartGrantForms;

namespace FormUI.Domain
{
    public static class FeatureToggles
    {
        public static bool WorkingOnGuardianBenefits(Sections section)
        {
            return section == Sections.GuardianBenefits || section == Sections.GuardianPartnerBenefits;
        }
    }
}