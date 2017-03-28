using System;
using System.ComponentModel;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExpectedChildren
    {
        [DisplayName("What date is the baby due?")]
        [HintText("For example 01 03 2017")]
        public DateTime? ExpectancyDate { get; set; }

        [DisplayName("Is more than 1 baby expected?")]
        [HintText("Please make sure that the document you are sending with this claim form tells us the number of babies that are expected")]
        [UiLength(2)]
        public int? ExpectedBabyCount { get; set; }
    }
}