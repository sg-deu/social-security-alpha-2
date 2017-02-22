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

            if (_maxLength.HasValue)
                input.Attr("maxlength", _maxLength);

            return input;
        }
    }
}