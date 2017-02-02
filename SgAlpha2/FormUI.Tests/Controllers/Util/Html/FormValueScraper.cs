using System;
using System.Collections.Generic;

namespace FormUI.Tests.Controllers.Util.Html
{
    public static class FormValueScraper
    {
        public static FormValue FromElement(ElementWrapper element)
        {
            var formValue = new FormValue(element.AttributeOrEmpty("name"));
            formValue.SetDisabled(element.HasAttribute("disabled"));

            switch (element.TagName.ToLower())
            {
                case "input":
                    return FromInput(element, formValue);

                case "textarea":
                    return FromTextArea(element, formValue);

                case "select":
                    return FromSelect(element, formValue);

                default:
                    throw new Exception("Unhandled tag: " + element.TagName);
            }
        }

        public static FormValue FromInput(ElementWrapper input, FormValue formValue)
        {
            var type = input.HasAttribute("type") ? input.Attribute("type").ToLower() : "text";

            formValue
                .SetValue(input.HasAttribute("value") ? input.Attribute("value") : null)
                .SetReadonly(input.HasAttribute("readonly"));

            if (type == "checkbox" || type == "radio")
            {
                if (string.IsNullOrEmpty(formValue.Value))
                    formValue.SetValue("on");

                if (!input.HasAttribute("checked"))
                    formValue.SetSend(false);
            }

            return formValue;
        }

        public static FormValue FromTextArea(ElementWrapper textArea, FormValue formValue)
        {
            formValue
                .SetValue(textArea.TextContent)
                .SetReadonly(textArea.HasAttribute("readonly"));

            return formValue;
        }

        public static FormValue FromSelect(ElementWrapper select, FormValue formValue)
        {
            var options = select.FindAll("option");
            var texts = new List<string>();
            var values = new List<string>();

            foreach (var option in options)
            {
                var text = option.TextContent;

                var value = option.HasAttribute("value")
                    ? option.Attribute("value")
                    : text;

                texts.Add(text);
                values.Add(value);

                if (option.HasAttribute("selected"))
                    formValue.SetValue(value);
            }

            formValue.SetConfinedValues(values, texts);

            return formValue;
        }
    }
}
