using System.Collections.Generic;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class Radios : InputControl
    {
        private IList<string>               _values;
        private IDictionary<string, string> _labels;

        public Radios(HtmlHelper helper, string id, string name, IList<string> values, IDictionary<string, string> labels) : base(helper, id, name)
        {
            _values = values;
            _labels = labels;
        }

        protected override HtmlTag CreateTag()
        {
            var vertical = _values.Count > 2;
            var radioClass = vertical ? "radio radio-block" : "radio radio-inline";

            var container = new DivTag().AddClasses(radioClass);

            foreach (var value in _values)
            {
                var label = CreateLabel(value);
                container.Append(label);
            }

            return container;
        }

        private HtmlTag CreateLabel(string value)
        {
            var label = new HtmlTag("label").AddClasses("radio-item");
            var input = new HtmlTag("input").Attr("type", "radio").Name(Name).Value(value);

            var labelText = _labels.ContainsKey(value) ? _labels[value] : value;
            var text = new HtmlTag("span").Text(labelText);

            return label.Append(input).Append(text);
        }
    }
}