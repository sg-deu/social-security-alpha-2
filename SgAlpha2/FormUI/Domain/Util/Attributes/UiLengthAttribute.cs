using System;

namespace FormUI.Domain.Util.Attributes
{
    public class UiLengthAttribute : Attribute
    {
        public UiLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public int MaxLength { get; private set; }
    }
}