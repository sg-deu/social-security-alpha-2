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
            return WorkingOnEvidenceSection(section);
        }

        public static bool WorkingOnEvidenceSection(ChangeOfCircsForm.Sections section)
        {
            return section == ChangeOfCircsForm.Sections.Evidence;
        }
    }
}