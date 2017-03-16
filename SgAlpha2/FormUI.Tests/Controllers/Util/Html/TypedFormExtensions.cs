using System;
using System.Linq.Expressions;
using System.Net;
using FluentAssertions;
using FormUI.Controllers.Helpers;
using FormUI.Domain.Util;
using FormUI.Tests.Controllers.Util.Http;
using Newtonsoft.Json;

namespace FormUI.Tests.Controllers.Util.Html
{
    public static class TypedFormExtensions
    {
        public static FormValue[] Get<T, U>(this TypedForm<T> form, Expression<Func<T, U>> property)
        {
            var formName = FormName(property);
            return form.Get(formName);
        }

        public static string GetText<T, U>(this TypedForm<T> form, Expression<Func<T, U>> property)
        {
            var formName = FormName(property);
            return form.GetSingle(formName).Value;
        }

        public static bool GetConfirm<T>(this TypedForm<T> form, Expression<Func<T, bool>> property)
        {
            var formName = FormName(property);
            return form.GetSingle(formName).Value == "True";
        }

        public static TypedForm<T> SetText<T, U>(this TypedForm<T> form, Expression<Func<T, U>> property, string value)
        {
            var formName = FormName(property);
            form.GetSingle(formName).SetValue(value);
            return form;
        }

        public static TypedForm<T> SetDate<T>(this TypedForm<T> form, Expression<Func<T, DateTime?>> property, string day, string month, string year)
        {
            var formName = FormName(property);
            form.GetSingle(formName + "_day").SetValue(day);
            form.GetSingle(formName + "_month").SetValue(month);
            form.GetSingle(formName + "_year").SetValue(year);
            return form;
        }

        public static TypedForm<T> SelectYes<T, U>(this TypedForm<T> form, Expression<Func<T, U>> property)
        {
            return form.SetText(property, "True");
        }

        public static TypedForm<T> SelectNo<T, U>(this TypedForm<T> form, Expression<Func<T, U>> property)
        {
            return form.SetText(property, "False");
        }

        public static TypedForm<T> SelectConfirm<T>(this TypedForm<T> form, Expression<Func<T, bool>> property, bool check)
        {
            var formName = FormName(property);
            form.GetSingle(formName).SetSend(check);
            return form;
        }

        public static AjaxAction[] OnChange<T>(this TypedForm<T> form, Expression<Func<T, object>> property, ISimulatedHttpClient client)
        {
            var formGroupSelector = "#" + FormName(property) + "_FormGroup";
            var formGroup = form.Element.Find(formGroupSelector);
            var changeUrl = formGroup.Attribute("data-ajax-change");
            form.SetAction(changeUrl);
            var ajaxResponse = form.Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));
            var json = ajaxResponse.Text;
            var settings = new JsonSerializerSettings { ContractResolver = new Repository.RepositoryContractResolver() };
            var ajaxActions = JsonConvert.DeserializeObject<AjaxAction[]>(json, settings);
            return ajaxActions;
        }

        public static string FormName(LambdaExpression property)
        {
            return property.GetExpressionText();
        }

        public static void WhenCheckedShows<T>(this TypedForm<T> form, Expression<Func<T, bool>> property, string elementId)
        {
            var idSelector = "#" + elementId;
            var element = form.Element.Find(idSelector);
            var name = FormName(property);
            element.Attribute("data-checkbox-checked-show").Should().Be(name, "element id {0} should be configured to show when {1} is checked", elementId, name);
        }

        public static void WhenUncheckedShows<T>(this TypedForm<T> form, Expression<Func<T, bool>> property, string elementId)
        {
            var idSelector = "#" + elementId;
            var element = form.Element.Find(idSelector);
            var name = FormName(property);
            element.Attribute("data-checkbox-checked-hide").Should().Be(name, "element id {0} should be configured to hide when {1} is checked", elementId, name);
        }

        public static void RadioShows<T>(this TypedForm<T> form, Expression<Func<T, bool?>> property, bool? value, string elementId)
        {
            var idSelector = "#" + elementId;
            var element = form.Element.Find(idSelector);
            var name = FormName(property);
            var stringValue = value.HasValue ? value.Value.ToString() : "";
            element.Attribute("data-radio-show-name").Should().Be(name, "element id {0} should be configured to show/hide for {1}", elementId, name);
            element.Attribute("data-radio-show-value").Should().Be(stringValue, "element id {0} should be configured to show when {1} value is {2}", elementId, name, stringValue);
        }
    }
}
