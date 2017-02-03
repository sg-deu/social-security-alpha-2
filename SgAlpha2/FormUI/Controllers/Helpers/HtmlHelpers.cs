using System.Web;
using System.Web.Mvc;
using HtmlTags;

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

            var linkText = new HtmlTag("span").AddClasses("link-text").Text(text);

            var anchor = new LinkTag(null, urlHelper.Content(actionUrl))
                .AddClasses("button", "button--primary")
                .Title(text)
                .Append(linkText);

            return anchor;
        }
    }
}