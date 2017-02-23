using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExistingChild
    {
        [DisplayName("First name")]
        public string       FirstName           { get; set; }

        [DisplayName("Surname or family name")]
        public string       Surname             { get; set; }

        [DisplayName("Date of Birth")]
        [HintText("For example, 18 03 1980")]
        public DateTime?    DateOfBirth         { get; set; }

        [DisplayName("Their relationship to  the child")]
        public string       RelationshipToChild { get; set; }

        [DisplayName("Child Benefit (if any)")]
        public bool?        ChildBenefit        { get; set; }

        [DisplayName("Do you have formal kinship care of this child?")]
        public bool?        FormalKinshipCare   { get; set; }
    }
}