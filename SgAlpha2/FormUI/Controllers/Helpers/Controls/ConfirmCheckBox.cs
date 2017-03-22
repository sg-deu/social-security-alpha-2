using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class ConfirmCheckBox : InputControl
    {
        private string  _labelText;
        private bool    _emphasise;
        private bool    _disabled;

        public ConfirmCheckBox(HtmlHelper helper, ControlContext controlContext, string labelText) : base(helper, controlContext)
        {
            _labelText = labelText;
        }

        public override bool HandlesMandatoryInline() { return true; }

        public ConfirmCheckBox Emphasise(bool emphasise = true)
        {
            _emphasise = emphasise;
            return this;
        }

        public ConfirmCheckBox Disabled(bool disabled = true)
        {
            _disabled = disabled;
            return this;
        }

        protected override HtmlTag CreateTag(bool inlineMandatory = false)
        {
            var container = new DivTag().AddClasses("confirm-check-box");

            if (_emphasise)
                container.AddClass("emphasise");

            var modelValue = RenderValue();

            var input = new HtmlTag("input").Attr("type", "checkbox").Name(ControlContext.Name).Value("True");

            if (_disabled)
                input.Attr("disabled", "disabled");

            if (modelValue != null && modelValue.ToLower() == "true")
                input.Attr("checked", "checked");

            var labelText = _labelText ?? ControlContext.Metadata.DisplayName ?? ControlContext.Metadata.PropertyName;
            var text = new HtmlTag("span");

            if (inlineMandatory)
                text.Append(NewMandatory());

            text.Append(new HtmlTag("span").Text(labelText));

            var label = new HtmlTag("label").Append(input).Append(text);

            return container.Append(label);
        }
    }
}