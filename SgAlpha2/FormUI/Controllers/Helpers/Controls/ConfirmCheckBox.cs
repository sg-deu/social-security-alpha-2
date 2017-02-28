using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class ConfirmCheckBox : InputControl
    {
        private string _labelText;

        public ConfirmCheckBox(ControlContext controlContext, string labelText) : base(controlContext)
        {
            _labelText = labelText;
        }

        protected override HtmlTag CreateTag()
        {
            var container = new DivTag().AddClasses("confirm-check-box");
            var modelValue = RenderValue();

            var input = new HtmlTag("input").Attr("type", "checkbox").Name(ControlContext.Name).Value("True");

            if (modelValue != null && modelValue.ToLower() == "true")
                input.Attr("checked", "checked");

            var labelText = _labelText ?? ControlContext.Metadata.DisplayName ?? ControlContext.Metadata.PropertyName;
            var text = new HtmlTag("span").Text(labelText);

            var label = new HtmlTag("label").Append(input).Append(text);
            return container.Append(label);
        }
    }
}