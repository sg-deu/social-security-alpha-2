using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public enum Relationship
    {
        [Description("Parent")]
        Parent,

        [Description("Adopted parent")]
        AdoptedParent,

        [Description("Parental Order")]
        ParentalOrder,

        [Description("Kinship carer")]
        KinshipCarer,

        [Description("Legal Guardian")]
        LegalGuardian,
    }
}