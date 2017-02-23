using FormUI.App_Start;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputText : InputControl
    {
        public int? _maxLength;

        public InputText(ControlContext controlContext) : base(controlContext)
        {
        }

        public InputText MaxLength(int? maxLength)
        {
            _maxLength = maxLength;
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

            var maxLength = _maxLength;

            if (!maxLength.HasValue && ControlContext.Metadata.AdditionalValues.ContainsKey(Metadata.MaxLength))
                maxLength = (int)ControlContext.Metadata.AdditionalValues[Metadata.MaxLength];

            if (maxLength.HasValue)
                input.Attr("maxlength", maxLength);

            return input;
        }
    }
}