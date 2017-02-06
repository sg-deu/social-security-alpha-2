using System.Web;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class Control : IHtmlString
    {
        public string ToHtmlString()
        {
            return ToTag().ToHtmlString();
        }

        public abstract HtmlTag ToTag();
    }
}