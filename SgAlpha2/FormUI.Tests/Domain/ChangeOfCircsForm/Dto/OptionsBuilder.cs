using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
{
    public static class OptionsBuilder
    {
        public static Options NewValid(Action<Options> mutator = null)
        {
            var value = new Options
            {
                OtherDetails = "some other details to change",
            };

            SetAllBool(value, true);

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static Options AllUnselected(this Options options)
        {
            SetAllBool(options, false);
            options.OtherDetails = null;
            return options;
        }

        private static void SetAllBool(Options options, bool set)
        {
            foreach (var prop in typeof(Options).GetProperties())
            {
                if (prop.PropertyType == typeof(bool))
                    prop.SetValue(options, set);
            }

            if (set)
            {
                // until these are implemented, they cannot be set to true
                options.ChangePartnerDetails = false;
                options.ChangeChildrenDetails = false;
            }
        }
    }
}
