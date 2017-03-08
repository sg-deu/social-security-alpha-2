using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputPassword : InputText
    {
        public InputPassword(HtmlHelper helper, ControlContext controlContext) : base(helper, controlContext)
        {
        }

        protected override HtmlTag CreateTag()
        {
            return base.CreateTag().Attr("type", "password");
        }
    }
}