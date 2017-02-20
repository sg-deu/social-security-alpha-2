using System;
using System.ComponentModel;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExpectedChildren
    {
        [DisplayName("If you are pregnant, please state the expectancy date")]
        [HintText("For example, 18 03 1980")]
        public DateTime? ExpectancyDate { get; set; }

        [DisplayName("If more than one baby is expected, please state how many are expected")]
        [HintText("Please make sure that the document you are sending with this claim form tells us the number of babies that are expected")]
        public int? ExpectedBabyCount { get; set; }
    }
}