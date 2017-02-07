using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputPassword : InputText
    {
        public InputPassword(HtmlHelper helper, string id, string name) : base(helper, id, name)
        {
        }

        protected override HtmlTag CreateTag()
        {
            return base.CreateTag().Attr("type", "password");
        }
    }
}