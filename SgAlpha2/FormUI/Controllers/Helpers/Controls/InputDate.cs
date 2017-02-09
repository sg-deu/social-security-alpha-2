﻿using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputDate : InputControl
    {
        public InputDate(ControlContext controlContext) : base(controlContext)
        {
        }

        protected override HtmlTag CreateTag()
        {
            // need this to trigger the model binding for the property
            var hidden = new HtmlTag("input")
                .Attr("type", "hidden")
                .Name(ControlContext.Name);

            var day = CreateDatePartTag("Day", 2);
            var month = CreateDatePartTag("Month", 2);
            var year = CreateDatePartTag("Year", 4);

            var container = new HtmlTag("div")
                .AddClasses("date-input")
                .Append(hidden)
                .Append(day)
                .Append(month)
                .Append(year);

            return container;
        }

        private HtmlTag CreateDatePartTag(string part, int maxLength)
        {
            var id = ControlContext.Id + "_" + part.ToLower();
            var name = ControlContext.Name + "_" + part.ToLower();

            var label = new HtmlTag("label").Attr("for", id).Text(part);

            var input = new HtmlTag("input")
                .Id(id)
                .Name(name)
                .Attr("type", "text")
                .Attr("maxlength", maxLength);

            var partContainer = new HtmlTag("div")
                .AddClasses(part.ToLower())
                .Append(label)
                .Append(input);

            return partContainer;
        }
    }
}