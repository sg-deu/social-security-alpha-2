using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExpectedChildren
    {
        [DisplayName("Is a baby expected?")]
        public bool?        IsBabyExpected          { get; set; }

        [DisplayName("When is the baby due?")]
        [HintText("For example 01 03 2017.")]
        public DateTime?    ExpectancyDate          { get; set; }

        [DisplayName("Is more than one baby expected?")]
        public bool?        IsMoreThan1BabyExpected { get; set; }

        [DisplayName("If yes, how many?")]
        [UiLength(2)]
        public int?         ExpectedBabyCount       { get; set; }
    }
}