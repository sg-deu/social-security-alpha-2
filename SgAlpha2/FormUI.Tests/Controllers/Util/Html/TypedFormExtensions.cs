using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FormUI.Tests.Controllers.Util.Html
{
    public static class TypedFormExtensions
    {
        public static string GetText<T>(this TypedForm<T> form, Expression<Func<T, string>> property)
        {
            var formName = FormName(property);
            return form.GetSingle(formName).Value;
        }

        public static TypedForm<T> SetText<T>(this TypedForm<T> form, Expression<Func<T, string>> property, string value)
        {
            var formName = FormName(property);
            form.GetSingle(formName).SetValue(value);
            return form;
        }

        public static TypedForm<T> SetDate<T>(this TypedForm<T> form, Expression<Func<T, DateTime>> property, string day, string month, string year)
        {
            var formName = FormName(property);
            form.GetSingle(formName + "_day").SetValue(day);
            form.GetSingle(formName + "_month").SetValue(month);
            form.GetSingle(formName + "_year").SetValue(year);
            return form;
        }

        public static string FormName(LambdaExpression property)
        {
            return ExpressionHelper.GetExpressionText(property);
        }
    }
}
