namespace FormUI.Domain.BestStartGrantForms.Responses
{
    public enum NextType
    {
        Ineligible = 0, // making this the default forces the dev to choose one of the others
        Section,
        Complete,
    }
}