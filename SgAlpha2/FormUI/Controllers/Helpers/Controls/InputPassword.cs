using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputPassword : InputText
    {
        public InputPassword(string id, string name) : base(id, name)
        {
        }

        protected override HtmlTag CreateTag()
        {
            return base.CreateTag().Attr("type", "password");
        }
    }
}