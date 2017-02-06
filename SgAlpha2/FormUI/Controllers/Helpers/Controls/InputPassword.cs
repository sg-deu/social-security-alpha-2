using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputPassword : InputText
    {
        public InputPassword(string id, string name) : base(id, name)
        {
        }

        public override HtmlTag ToTag()
        {
            return base.ToTag().Attr("type", "password");
        }
    }
}