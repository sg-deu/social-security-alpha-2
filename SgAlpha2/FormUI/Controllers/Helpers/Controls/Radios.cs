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
            var container = new DivTag().AddClasses("radio-inline");

            foreach (var value in _values)
            {
                var label = CreateLabel(value);
                container.Append(label);
            }

            return container;
        }

        private HtmlTag CreateLabel(string value)
        {
            var label = new HtmlTag("label").AddClasses("radio-inline-item");
            var input = new HtmlTag("input").Attr("type", "radio").Name(Name);
            var text = new HtmlTag("span").Text(value);
            return label.Append(input).Append(text);
        }
    }
}