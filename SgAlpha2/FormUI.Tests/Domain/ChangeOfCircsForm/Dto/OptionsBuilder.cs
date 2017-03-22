using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
{
    public class OptionsBuilder
    {
        public static Options NewValid(Action<Options> mutator = null)
        {
            var value = new Options
            {
                ChangePersonalDetails = true,
                Other = true,
                OtherDetails = "some other details to change",
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
