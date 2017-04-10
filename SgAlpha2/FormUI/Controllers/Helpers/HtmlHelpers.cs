using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using FormUI.Controllers.Helpers.Controls;
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

        public static IHtmlString BackLink<T>(this HtmlHelper<T> helper, string actionUrl)
        {
            if (string.IsNullOrWhiteSpace(actionUrl))
                return null;

            var urlHelper = helper.UrlHelper();

            return new LinkTag("Back", urlHelper.Content(actionUrl));
        }

        // TODO: Appreciate this is not he right place for this (or if can use 'PartialFor<>' below) but will do in short term
        public static IHtmlString PartialDeclarationForGuardian<T>(this HtmlHelper<T> helper, bool isUnder16)
        {
            if (isUnder16)
            {
                return helper.Partial(FormUI.Controllers.Shared.SharedViews.LegalStatementU16);
            }
            else
            {
                return helper.Partial(FormUI.Controllers.Shared.SharedViews.LegalStatement);
            }
        }

        public static IHtmlString PartialFor<T, TViewModel>(this HtmlHelper<T> helper, Expression<Func<T, TViewModel>> property, string view, Func<TViewModel, object> modelFactory = null)
            where TViewModel : class
        {
            var model = (TViewModel)ModelMetadata.FromLambdaExpression(property, helper.ViewData).Model;
            var viewModel = modelFactory != null ? modelFactory(model) : model;

            if (viewModel == null)
                return null;

            var prefix = property.GetExpressionText();
            var templateInfo = new TemplateInfo { HtmlFieldPrefix = prefix };
            return helper.Partial(view, viewModel, new ViewDataDictionary(helper.ViewData) { TemplateInfo = templateInfo });
        }

        public static ScopedHtmlHelper<T> VisibleWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property, bool visible, Action<HtmlTag> mutator = null)
        {
            var propertyName = property.GetExpressionText();
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);

            var modelChecked = helper.ViewData.ModelState.ContainsKey(name);
            
            if (!modelChecked)
            {
                var modelValue = ModelMetadata.FromLambdaExpression(property, helper.ViewData).Model;
                modelChecked = modelValue != null && (bool)modelValue;
            }

            var container = new DivTag().NoClosingTag();

            if (visible && !modelChecked || !visible && modelChecked)
                container.AddClasses("show-hide-hidden");

            if (visible)
                container.Attr("data-checkbox-checked-show", name);
            else
                container.Attr("data-checkbox-checked-hide", name);

            if (mutator != null)
                mutator(container);

            helper.ViewContext.Writer.Write(container.ToHtmlString());

            return new ScopedHtmlHelper<T>(helper, () =>
            {
                helper.ViewContext.Writer.Write($"</{container.TagName()}>");
            });
        }

        public static ScopedHtmlHelper<T> VisibleWhenValue<T>(this HtmlHelper<T> helper, Expression<Func<T, bool?>> property, string value, Action<HtmlTag> mutator = null)
        {
            var propertyName = property.GetExpressionText();
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);

            string modelValue = null;
            var modelState = helper.ViewData.ModelState;

            if (modelState.ContainsKey(name))
                modelValue = (modelState[name].Value?.AttemptedValue ?? "").ToString();
            else
                modelValue = (ModelMetadata.FromLambdaExpression(property, helper.ViewData).Model ?? "").ToString();

            var container = new DivTag().NoClosingTag();

            if (modelValue != value)
                container.AddClasses("show-hide-hidden");

            container.Attr("data-radio-show-name", name);
            container.Attr("data-radio-show-value", value);

            if (mutator != null)
                mutator(container);

            helper.ViewContext.Writer.Write(container.ToHtmlString());

            return new ScopedHtmlHelper<T>(helper, () =>
            {
                helper.ViewContext.Writer.Write($"</{container.TagName()}>");
            });
        }

        public static ScopedHtmlHelper<T> HideWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property, Action<HtmlTag> mutator = null)
        {
            return helper.VisibleWhenChecked(property, false, mutator);
        }

        public static ScopedHtmlHelper<T> ShowWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property, Action<HtmlTag> mutator = null)
        {
            return helper.VisibleWhenChecked(property, true, mutator);
        }

        public static ScopedHtmlHelper<T> ShowWhenYes<T>(this HtmlHelper<T> helper, Expression<Func<T, bool?>> property, Action<HtmlTag> mutator = null)
        {
            return helper.VisibleWhenValue(property, true.ToString(), mutator);
        }

        public static ScopedHtmlHelper<T> ShowWhenNo<T>(this HtmlHelper<T> helper, Expression<Func<T, bool?>> property, Action<HtmlTag> mutator = null)
        {
            return helper.VisibleWhenValue(property, false.ToString(), mutator);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, Expression<Func<T, string>> property)
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, string>> property)
        {
            return Answer.For(helper, null, property, p => p);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, Expression<Func<T, DateTime?>> property)
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, DateTime?>> property)
        {
            return Answer.For(helper, null, property, p => p.HasValue
                ? p.Value.ToString("dd MMM yyyy")
                : null);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property)
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, bool>> property)
        {
            return Answer.For(helper, null, property, p => p ? "Yes" : "No");
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, Expression<Func<T, bool?>> property)
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, bool?>> property)
        {
            return Answer.For(helper, null, property, p => p.HasValue
                ? p.Value ? "Yes" : "No"
                : null);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, Expression<Func<T, int?>> property)
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, int?>> property)
        {
            return Answer.For(helper, null, property, p => p.HasValue
                ? p.Value.ToString()
                : null);
        }

        public static Answer AnswerFor<T, TEnum>(this HtmlHelper<T> helper, Expression<Func<T, TEnum?>> property)
            where TEnum : struct
        {
            return helper.AnswerFor(null, property);
        }

        public static Answer AnswerFor<T, TEnum>(this HtmlHelper<T> helper, string labelText, Expression<Func<T, TEnum?>> property)
            where TEnum : struct
        {
            return Answer.For(helper, null, property, p => ReflectHelper.GetEnumDescription(p));
        }
    }
}