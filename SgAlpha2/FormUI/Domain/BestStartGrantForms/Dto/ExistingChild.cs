using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExistingChild
    {
        [DisplayName("First name")]
        public string           FirstName               { get; set; }

        [DisplayName("Surname or family name")]
        public string           Surname                 { get; set; }

        [DisplayName("Child's date of birth")]
        [HintText("For example 01 03 2010.")]
        public DateTime?        DateOfBirth             { get; set; }

        [DisplayName("Your relationship to the child")]
        public Relationship?    Relationship            { get; set; }

        [DisplayName("Do you receive Child Benefit for this child?")]
        public bool?            ChildBenefit            { get; set; }

        [DisplayName("Explain why you are not getting child benefit")]
        [UiLength(400)]
        public string           NoChildBenefitReason    { get; set; }
    }
}