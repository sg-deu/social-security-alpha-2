using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputDate : InputControl
    {
        public InputDate(string id, string name) : base(id, name)
        {
        }

        protected override HtmlTag CreateTag()
        {
            var container = new HtmlTag("span");

            //var input = new HtmlTag("input")
            //    .Id(Id)
            //    .Name(Name)
            //    .Attr("type", "text")
            //    .AddClasses("form-control");

            return container;
        }
    }
}