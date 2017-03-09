using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public enum YesNoDk
    {
        [Description("Yes")]
        Yes,

        [Description("No")]
        No,

        [Description("Don't know")]
        DontKnow,
    }
}