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
            return WorkingOnDeclaration(section);
        }

        public static bool WorkingOnDeclaration(ChangeOfCircsForm.Sections section)
        {
            return section == ChangeOfCircsForm.Sections.Declaration;
        }
    }
}