using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FormUI.App_Start;
using FormUI.Controllers.Helpers.Controls;
using HtmlTags;

namespace FormUI.Controllers.Helpers
{
    public static class FormHelpers
    {
        public static ScopedHtmlHelper<TPostModel> FormFor<TViewModel, TPostModel>(this HtmlHelper<TViewModel> helper, TPostModel postModel)
        {
            var form = new FormTag().AddClasses("sg-form").NoClosingTag();
            helper.ViewContext.Writer.Write(form.ToHtmlString());

            var newHelper = helper.ForModel(postModel);
            return new ScopedHtmlHelper<TPostModel>(newHelper, () =>
            {
                helper.ViewContext.Writer.Write($"</{form.TagName()}>");
            });
        }

        public static HtmlHelper<TPostModel> ForModel<TViewModel, TPostModel>(this HtmlHelper<TViewModel> helper, TPostModel postModel)
        {
            var viewData = new ViewDataDictionary(helper.ViewData);
            viewData.Model = postModel;
            var data = new ViewDataContainer { ViewData = viewData };
            var newHelper = new HtmlHelper<TPostModel>(helper.ViewContext, data);
            return newHelper;
        }

        public static IHtmlString ButtonSubmit<T>(this HtmlHelper<T> helper)
        {
            return helper.ButtonSubmit("Submit");
        }

        public static IHtmlString ButtonSubmitNext<T>(this HtmlHelper<T> helper, bool isFinalPage)
        {
            var text = isFinalPage ? "Submit" : "Next";
            return helper.ButtonSubmit(text);
        }

        public static IHtmlString ButtonSubmit<T>(this HtmlHelper<T> helper, string text)
        {
            var button = new HtmlTag("button")
                .AddClasses("button button--primary")
                .Text(text);

            var formGroup = new DivTag().AddClasses("form-group").Append(button);

            return formGroup;
        }

        public static FormRow<InputText> LabelledInputText<T>(this HtmlHelper<T> helper, Expression<Func<T, string>> property, string hintText = null)
        {
            return helper.LabelledInputText(null, property, hintText);
        }

        public static FormRow<InputText> LabelledInputText<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property, string hintText = null)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputText(helper, controlContext));
        }

        public static FormRow<InputPassword> LabelledInputPassword<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputPassword(helper, controlContext));
        }

        public static FormRow<InputDate> LabelledInputDate<T>(this HtmlHelper<T> helper, Expression<Func<T, DateTime?>> property)
        {
            return helper.LabelledInputDate(null, property);
        }

        public static FormRow<InputDate> LabelledInputDate<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, DateTime?>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputDate(helper, controlContext));
        }

        public static FormRow<InputText> LabelledInputInt<T>(this HtmlHelper<T> helper, Expression<Func<T, int?>> property)
        {
            return helper.LabelledInputInt(null, property);
        }

        public static FormRow<InputText> LabelledInputInt<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, int?>> property)
        {
            return helper.LabelledControl(labelText, property, controlContext =>
                new InputText(helper, controlContext));
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, Expression<Func<T, Nullable<TEnum>>> property)
             where TEnum : struct
        {
            return helper.LabelledRadio(null, property);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, Expression<Func<T, Nullable<TEnum>>> property, IDictionary<TEnum, string> descriptions)
             where TEnum : struct
        {
            return helper.LabelledRadio(null, property, descriptions);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, Nullable<TEnum>>> property)
             where TEnum : struct
        {
            var descriptions = ReflectHelper.GetEnumDescriptions<TEnum>();
            return helper.LabelledRadio(labelText, property, descriptions);
        }

        public static FormRow<Radios> LabelledRadio<T, TEnum>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, Nullable<TEnum>>> property, IDictionary<TEnum, string> descriptions)
             where TEnum : struct
        {
            var values = typeof(TEnum).GetEnumStringValues();
            var descriptionTexts = descriptions.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);

            return helper.LabelledControl(labelText, property, controlContext =>
                new Radios(helper, controlContext, values, descriptionTexts));
        }

        public static FormRow<Radios> LabelledRadioYesNo<T>(this HtmlHelper<T> helper, Expression<Func<T, bool?>> property)
        {
            return helper.LabelledRadioYesNo(null, property);
        }

        public static FormRow<Radios> LabelledRadioYesNo<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, bool?>> property)
        {
            var values = new List<string> { "True", "False" };
            var descriptionTexts = new Dictionary<string, string> { { "True", "Yes" }, { "False", "No" }, };

            return helper.LabelledControl(labelText, property, controlContext =>
                new Radios(helper, controlContext, values, descriptionTexts));
        }

        public static FormRow<ConfirmCheckBox> LabelledConfirmCheckBox<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property)
        {
            return helper.LabelledConfirmCheckBox(null, property);
        }

        public static FormRow<ConfirmCheckBox> LabelledConfirmCheckBox<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, bool>> property)
        {
            return helper.LabelledControl("", property, controlContext =>
                new ConfirmCheckBox(helper, controlContext, labelText));
        }

        public static HtmlTag FormButton<T>(this HtmlHelper<T> helper, string name, string label)
        {
            return helper.FormButton(name, null, label);
        }

        public static HtmlTag FormButton<T>(this HtmlHelper<T> helper, string name, string value, string label)
        {
            var div = new DivTag().AddClasses("form-group");
            var button = new HtmlTag("button").AddClasses("button", "button--primary").Text(label);

            if (!string.IsNullOrWhiteSpace(name))
                button.Name(name);

            if (!string.IsNullOrWhiteSpace(value))
                button.Value(value);

            return div.Append(button);
        }

        public delegate TControl ControlFactory<TControl>(ControlContext controlContext);

        private static FormRow<TControl> LabelledControl<TModel, TProperty, TControl>(this HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProperty>> property, ControlFactory<TControl> factory)
            where TControl : Control
        {
            var propertyName = property.GetExpressionText();
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);
            var id = TagBuilder.CreateSanitizedId(name);
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

            var formRow = new FormRow<TControl>(helper, id, labelText, control);

            if (metaData.AdditionalValues.ContainsKey(Metadata.HintText))
                formRow.Hint((string)metaData.AdditionalValues[Metadata.HintText]);

            if (metaData.AdditionalValues.ContainsKey(Metadata.MaxLength))
            {
                int maxLength = (int)metaData.AdditionalValues[Metadata.MaxLength];

                if (maxLength <= 5)
                    formRow.Width(ControlWidth.Small);
                else if (maxLength <= 25)
                    formRow.Width(ControlWidth.Medium);
            }

            return formRow;
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }
    }
}