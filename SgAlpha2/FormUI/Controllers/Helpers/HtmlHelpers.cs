﻿using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
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

        public static IHtmlString PartialFor<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> property, string view)
        {
            var prefix = property.GetExpressionText();
            var templateInfo = new TemplateInfo { HtmlFieldPrefix = prefix };
            var metaData = ModelMetadata.FromLambdaExpression(property, helper.ViewData);
            return helper.Partial(view, metaData.Model, new ViewDataDictionary(helper.ViewData) { TemplateInfo = templateInfo });
        }

        public static ScopedHtmlHelper<T> VisibleWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property, bool visible)
        {
            var container = new DivTag().NoClosingTag();

            if (!visible)
                container.AddClasses("show-hide-hidden");

            helper.ViewContext.Writer.Write(container.ToHtmlString());

            return new ScopedHtmlHelper<T>(helper, () =>
            {
                helper.ViewContext.Writer.Write($"</{container.TagName()}>");
            });
        }

        public static ScopedHtmlHelper<T> HideWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property)
        {
            return helper.VisibleWhenChecked(property, false);
        }

        public static ScopedHtmlHelper<T> ShowWhenChecked<T>(this HtmlHelper<T> helper, Expression<Func<T, bool>> property)
        {
            return helper.VisibleWhenChecked(property, true);
        }
    }
}