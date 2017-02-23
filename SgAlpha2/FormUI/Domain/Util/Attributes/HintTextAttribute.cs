using System;

namespace FormUI.Domain.Util.Attributes
{
    public class HintTextAttribute : Attribute
    {
        public HintTextAttribute(string hintText)
        {
            HintText = hintText;
        }

        public string HintText { get; private set; }
    }
}