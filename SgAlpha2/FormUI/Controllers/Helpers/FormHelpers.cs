using System;
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
            return helper.LabelledControl(labelText, property, (id, name) =>
                new InputText(id, name));
        }

        public static FormRow<InputPassword> LabelledInputPassword<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            return helper.LabelledControl(labelText, property, (id, name) =>
                new InputPassword(id, name));
        }

        public delegate TControl ControlFactory<TControl>(string id, string name);

        private static FormRow<TControl> LabelledControl<TModel, TProperty, TControl>(this HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProperty>> property, ControlFactory<TControl> factory)
            where TControl : Control
        {
            var name = ExpressionHelper.GetExpressionText(property);
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name));

            var label = new HtmlTag("label").Text(labelText).Attr("for", id);

            var control = factory(id, name);

            var formRow = new FormRow<TControl>(id, labelText, control);

            return formRow;
        }
    }
}