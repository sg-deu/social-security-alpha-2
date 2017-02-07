using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputText : InputControl
    {
        public InputText(HtmlHelper helper, string id, string name) : base(helper, id, name)
        {
        }

        protected override HtmlTag CreateTag()
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