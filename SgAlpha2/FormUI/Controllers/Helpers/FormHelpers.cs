﻿using System;
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
            var label = new HtmlTag("label").Text(labelText);

            var input = new HtmlTag("input").Attr("type", "text").AddClasses("form-control");

            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(input);

            var formGroup = new DivTag()
                .AddClasses("form-group")
                .Append(label)
                .Append(inputWrapper);

            if (!string.IsNullOrWhiteSpace(hintText))
            {
                var hint = new HtmlTag("p").AddClasses("help-block").AppendHtml(hintText);
                label.After(hint);
            }

            return formGroup;
        }
    }
}