using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using HtmlTags;

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
            var button = new HtmlTag("button")
                .AddClasses("button button--primary")
                .Text(text);

            var formGroup = new DivTag().AddClasses("form-group").Append(button);

            return formGroup;
        }

        public static IHtmlString LabelledInputText<T>(this HtmlHelper<T> helper, string label, Expression<Func<T, string>> property)
        {
            var formGroup = new DivTag().AddClasses("form-group");
            return formGroup;
        }
    }
}