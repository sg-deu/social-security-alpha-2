using System.Web;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers
{
    public static class HtmlHelpers
    {
        public static UrlHelper UrlHelper<T>(this HtmlHelper<T> helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }

        public static IHtmlString ButtonLinkNext<T>(this HtmlHelper<T> helper, string actionUrl)
        {
            return helper.ButtonLink(actionUrl, "Next");
        }

        public static IHtmlString ButtonLink<T>(this HtmlHelper<T> helper, string actionUrl, string text)
        {
            var urlHelper = helper.UrlHelper();

            var html =
                $"<a class=\"button button--primary\" title=\"Next\" href=\"{urlHelper.Content(actionUrl)}\">"
                    + $"<span class=\"link-text\">{text}</span>"
                  + "</a>";

            return new MvcHtmlString(html);
        }
    }
}