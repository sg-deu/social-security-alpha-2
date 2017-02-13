using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public enum ContactPreference
    {
        [Description("Email")]
        Email,

        [Description("Phone")]
        Phone,

        [Description("Text message")]
        Text,
    }
}