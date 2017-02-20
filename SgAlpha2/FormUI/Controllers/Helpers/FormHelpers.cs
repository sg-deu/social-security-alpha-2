using System;
using System.Collections.Generic;
using System.Linq;
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
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputText(controlContext));
        }

        public static FormRow<InputText> LabelledInputText<T>(this HtmlHelper<T> helper, Expression<Func<T, string>> property, string hintText = null)
        {
            return helper.LabelledControl(null, property, controlContext =>
                new InputText(controlContext));
        }

        public static FormRow<InputPassword> LabelledInputPassword<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputPassword(controlContext));
        }

        public static FormRow<InputDate> LabelledInputDate<T>(this HtmlHelper<T> helper, Expression<Func<T, DateTime?>> property)
        {
            return helper.LabelledControl(null, property, controlContext =>
                new InputDate(controlContext));
        }

        public static FormRow<InputDate> LabelledInputDate<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, DateTime?>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputDate(controlContext));
        }

        public static FormRow<InputText> LabelledInputInt<T>(this HtmlHelper<T> helper, Expression<Func<T, int?>> property)
        {
            return helper.LabelledInputInt(null, property);
        }

        public static FormRow<InputText> LabelledInputInt<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, int?>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputText(controlContext));
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, Expression<Func<T, Nullable<TEnum>>> property)
             where TEnum : struct
        {
            return helper.LabelledRadio(null, property);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, Nullable<TEnum>>> property)
             where TEnum : struct
        {
            var descriptions = ReflectHelper.GetEnumDescriptions<TEnum>();
            return helper.LabelledRadio(labelText, property, descriptions);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, Expression<Func<T, Nullable<TEnum>>> property, IDictionary<TEnum, string> descriptions)
             where TEnum : struct
        {
            return helper.LabelledRadio(null, property, descriptions);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, Nullable<TEnum>>> property, IDictionary<TEnum, string> descriptions)
             where TEnum : struct
        {
            var values = typeof(TEnum).GetEnumStringValues();
            var descriptionTexts = descriptions.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);

            return helper.LabelledControl(labelText, property, controlContext =>
                new Radios(controlContext, values, descriptionTexts));
        }

        public delegate TControl ControlFactory<TControl>(ControlContext controlContext);

        private static FormRow<TControl> LabelledControl<TModel, TProperty, TControl>(this HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProperty>> property, ControlFactory<TControl> factory)
            where TControl : Control
        {
            var name = property.GetExpressionText();
            var id = TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name));
            var metaData = ModelMetadata.FromLambdaExpression(property, helper.ViewData);

            var controlContext = new ControlContext
            {
               Helper   = helper,
               Id       = id,
               Name     = name,
               Metadata = metaData,
            };

            labelText = labelText ?? metaData.DisplayName ?? metaData.PropertyName;
            var label = new HtmlTag("label").Text(labelText).Attr("for", id);

            var control = factory(controlContext);

            var formRow = new FormRow<TControl>(id, labelText, control);

            if (metaData.AdditionalValues.ContainsKey("HintText"))
                formRow.Hint((string)metaData.AdditionalValues["HintText"]);

            return formRow;
        }
    }
}