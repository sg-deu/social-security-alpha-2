using System;

namespace FormUI.Domain.Util.Attributes
{
    public class UiInputMaskAttribute : Attribute
    {
        public UiInputMaskAttribute(InputMasks inputMask)
        {
            InputMask = inputMask;
        }

        public InputMasks InputMask { get; private set; }
    }
}