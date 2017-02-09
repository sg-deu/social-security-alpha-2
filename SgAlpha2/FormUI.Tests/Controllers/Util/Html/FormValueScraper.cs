using System;
using System.Collections.Generic;
using System.Linq;

namespace FormUI.Tests.Controllers.Util.Html
{
    public static class FormValueScraper
    {
        public static IEnumerable<FormValue> FromElements(IEnumerable<ElementWrapper> elements)
        {
            var grouped = elements
                .GroupBy(e => e.AttributeOrEmpty("name"))
                .ToDictionary(g => g.Key, g => g.ToList());

            var values = new List<FormValue>();

            foreach (var name in grouped.Keys)
            {
                var elementsWithName = grouped[name];

                if (string.IsNullOrWhiteSpace(name) || elementsWithName.Count == 1)
                {
                    foreach (var element in grouped[name])
                    {
                        var value = FromElement(element);
                        values.Add(value);
                    }
                }
                else
                {
                    var value = FromMultipleElements(name, elementsWithName);

                    if (value != null)
                        values.Add(value);
                }
            }

            return values;
        }

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

        public static FormValue FromMultipleElements(string name, IList<ElementWrapper> elements)
        {
            var enabledElements = elements
                .Where(e => !e.HasAttribute("disabled"))
                .ToList();

            if (enabledElements.Count == 0)
                return null;

            var checkedValues = enabledElements.Where(e => e.HasAttribute("checked")).ToList();

            var formValue = new FormValue(name)
                .SetConfinedValues(enabledElements.Select(e => e.Attribute("value")).ToList());

            if (checkedValues.Count > 0)
                formValue.SetValue(string.Join(",", checkedValues.Select(e => e.Attribute("value"))));

            return formValue;
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
