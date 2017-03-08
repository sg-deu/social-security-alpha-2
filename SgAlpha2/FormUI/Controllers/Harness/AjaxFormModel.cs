using System;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Controllers.Harness
{
    public class AjaxFormModel
    {
        [HintText("Enter a date to see the hidden fields")]
        public DateTime? Date   { get; set; }

        public string String1   { get; set; }
        public string String2   { get; set; }
    }
}