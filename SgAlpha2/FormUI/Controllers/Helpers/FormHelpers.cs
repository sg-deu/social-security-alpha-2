using System.Web;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers
{
    public static class FormHelpers
    {
        public static IHtmlString ButtonSubmit<T>(this HtmlHelper<T> helper)
        {
            return helper.ButtonSubmit("Submit");
        }

        public static IHtmlString ButtonSubmit<T>(this HtmlHelper<T> helper, string text)
        {
            var html =
                "<div class=\"form-group\">"
                    + $"<button data-gtm=\"form-submit\" class=\"button button--primary\">{text}</button>"
                + "</div>";

            return new MvcHtmlString(html);
        }
    }
}