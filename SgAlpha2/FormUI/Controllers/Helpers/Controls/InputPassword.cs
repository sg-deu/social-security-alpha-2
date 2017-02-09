using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputPassword : InputText
    {
        public InputPassword(ControlContext controlContext) : base(controlContext)
        {
        }

        protected override HtmlTag CreateTag()
        {
            return base.CreateTag().Attr("type", "password");
        }
    }
}