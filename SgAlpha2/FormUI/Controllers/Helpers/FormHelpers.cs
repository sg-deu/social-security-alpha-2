﻿using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FormUI.Controllers.Helpers.Controls;
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

        public static FormRow<InputText> LabelledInputText<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property, string hintText = null)
        {
            return helper.LabelledControl(labelText, property, (h, id, name) =>
                new InputText(h, id, name));
        }

        public static FormRow<InputPassword> LabelledInputPassword<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            return helper.LabelledControl(labelText, property, (h, id, name) =>
                new InputPassword(h, id, name));
        }

        public static FormRow<InputDate> LabelledInputDate<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, DateTime>> property)
        {
            return helper.LabelledControl(labelText, property, (h, id, name) =>
                new InputDate(h, id, name));
        }

        public static FormRow<InputText> LabelledInputInt<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, int>> property)
        {
            return helper.LabelledControl(labelText, property, (h, id, name) =>
                new InputText(h, id, name));
        }

        public delegate TControl ControlFactory<TControl>(HtmlHelper helper, string id, string name);

        private static FormRow<TControl> LabelledControl<TModel, TProperty, TControl>(this HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProperty>> property, ControlFactory<TControl> factory)
            where TControl : Control
        {
            var name = ExpressionHelper.GetExpressionText(property);
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name));

            var label = new HtmlTag("label").Text(labelText).Attr("for", id);

            var control = factory(helper, id, name);

            var formRow = new FormRow<TControl>(id, labelText, control);

            return formRow;
        }
    }
}