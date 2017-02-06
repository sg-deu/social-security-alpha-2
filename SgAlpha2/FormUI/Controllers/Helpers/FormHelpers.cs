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

        public static IHtmlString LabelledInputText<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property, string hintText = null)
        {
            var name = ExpressionHelper.GetExpressionText(property);
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name));

            var label = new HtmlTag("label").Text(labelText).Attr("for", id);

            var input = new HtmlTag("input").Attr("type", "text").AddClasses("form-control").Id(id).Name(name);

            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(input);

            var formGroup = new DivTag()
                .AddClasses("form-group")
                .Append(label)
                .Append(inputWrapper);

            if (!string.IsNullOrWhiteSpace(hintText))
            {
                var hint = new HtmlTag("p").AddClasses("help-block").AppendHtml(hintText);
                label.Append(hint);
            }

            return formGroup;
        }

        public static IHtmlString LabelledInputPassword<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            var name = ExpressionHelper.GetExpressionText(property);
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name));

            var label = new HtmlTag("label").Text(labelText).Attr("for", id);

            var input = new HtmlTag("input").Attr("type", "password").AddClasses("form-control").Id(id).Name(name);

            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(input);
            var hintText = string.Empty;

            var formGroup = new DivTag()
                .AddClasses("form-group")
                .Append(label)
                .Append(inputWrapper);

            if (!string.IsNullOrWhiteSpace(hintText))
            {
                var hint = new HtmlTag("p").AddClasses("help-block").AppendHtml(hintText);
                label.Append(hint);
            }

            return formGroup;
        }
    }
}