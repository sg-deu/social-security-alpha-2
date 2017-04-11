namespace FormUI.Domain
{
    public static class FeatureToggles
    {
        public static bool SkipWorkInProgressSection(BestStartGrantForms.Sections section)
        {
            return false;
        }

        public static bool SkipWorkInProgressSection(ChangeOfCircsForm.Sections section)
        {
            return WorkingOnExpectedChildren(section);
        }

        public static bool WorkingOnExpectedChildren(ChangeOfCircsForm.Sections section)
        {
            return section == ChangeOfCircsForm.Sections.ExpectedChildren || section == ChangeOfCircsForm.Sections.HealthProfessional;
        }
    }
}