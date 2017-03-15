using System.Web.Mvc;
using FormUI.App_Start;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputText : InputControl
    {
        private int? _maxLength;
        private bool _disabled;

        public InputText(HtmlHelper helper, ControlContext controlContext) : base(helper, controlContext)
        {
        }

        public InputText MaxLength(int? maxLength)
        {
            _maxLength = maxLength;
            return this;
        }

        public InputText Disabled(bool disabled = true)
        {
            _disabled = disabled;
            return this;
        }

        protected override HtmlTag CreateTag()
        {
            var input = new HtmlTag("input")
                .Id(ControlContext.Id)
                .Name(ControlContext.Name)
                .Value(RenderValue())
                .Attr("type", "text")
                .AddClasses("form-control");

            var additionalValues = ControlContext.Metadata.AdditionalValues;
            var maxLength = _maxLength;

            if (!maxLength.HasValue && additionalValues.ContainsKey(Metadata.MaxLength))
                maxLength = (int)additionalValues[Metadata.MaxLength];

            if (maxLength.HasValue)
                input.Attr("maxlength", maxLength);

            if (_disabled)
                input.Attr("disabled", "disabled");

            if (additionalValues.ContainsKey(Metadata.InputMask))
                input.Attr("data-input-mask", additionalValues[Metadata.InputMask]);

            return input;
        }
    }
}