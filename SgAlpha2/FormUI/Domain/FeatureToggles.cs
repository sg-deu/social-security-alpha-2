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
            return WorkingOnIdentify(section);
        }

        public static bool WorkingOnIdentify(ChangeOfCircsForm.Sections section)
        {
            return section == ChangeOfCircsForm.Sections.Identity;
        }
    }
}