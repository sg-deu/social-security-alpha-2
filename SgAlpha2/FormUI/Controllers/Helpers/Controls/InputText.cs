using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputText : InputControl
    {
        public InputText(ControlContext controlContext) : base(controlContext)
        {
        }

        protected override HtmlTag CreateTag()
        {
            var input = new HtmlTag("input")
                .Id(ControlContext.Id)
                .Name(ControlContext.Name)
                .Attr("type", "text")
                .AddClasses("form-control");

            return input;
        }
    }
}