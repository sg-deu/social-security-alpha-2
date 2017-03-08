using System.Collections.Generic;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class Radios : InputControl
    {
        private IList<string>               _values;
        private IDictionary<string, string> _labels;

        public Radios(HtmlHelper helper, ControlContext controlContext, IList<string> values, IDictionary<string, string> labels) : base(helper, controlContext)
        {
            _values = values;
            _labels = labels;
        }

        protected override HtmlTag CreateTag()
        {
            var vertical = _values.Count > 2;
            var radioClass = vertical ? "radio radio-block" : "radio radio-inline";

            var container = new DivTag().AddClasses(radioClass);
            var modelValue = RenderValue();

            foreach (var value in _values)
            {
                var label = CreateLabel(value, modelValue);
                container.Append(label);
            }

            return container;
        }

        private HtmlTag CreateLabel(string value, string modelValue)
        {
            var label = new HtmlTag("label").AddClasses("radio-item");
            var input = new HtmlTag("input").Attr("type", "radio").Name(ControlContext.Name).Value(value);

            if (value == modelValue)
                input.Attr("checked", "true");

            var labelText = _labels.ContainsKey(value) ? _labels[value] : value;
            var text = new HtmlTag("span").Text(labelText);

            return label.Append(input).Append(text);
        }
    }
}