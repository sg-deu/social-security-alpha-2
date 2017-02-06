using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputText : InputControl
    {
        public InputText(string id, string name) : base(id, name)
        {
        }

        public override HtmlTag ToTag()
        {
            var input = new HtmlTag("input")
                .Id(Id)
                .Name(Name)
                .Attr("type", "text")
                .AddClasses("form-control");

            return input;
        }
    }
}