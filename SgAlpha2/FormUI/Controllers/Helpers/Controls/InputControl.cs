using System.Web.Mvc;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class InputControl : Control
    {
        public InputControl(HtmlHelper helper, string id, string name)
        {
            Helper = helper;
            Id = id;
            Name = name;
        }

        public HtmlHelper   Helper  { get; protected set; }
        public string       Id      { get; protected set; }
        public string       Name    { get; protected set; }
    }
}