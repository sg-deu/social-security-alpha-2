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
            var name = GetExpressionName(helper, property);

            var label = new HtmlTag("label").Text(labelText).Attr("for", name);

            var input = new HtmlTag("input").Attr("type", "text").AddClasses("form-control").Id(name).Name(name);

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

        private static string GetExpressionName<T>(HtmlHelper<T> helper, LambdaExpression expression)
        {
            return GetExpressionText(expression.Body);
        }

        private static string GetExpressionText(LambdaExpression expression)
        {
            return GetExpressionText(expression.Body);
        }

        private static string GetExpressionText(Expression expression)
        {
            var me = GetMemberExpression(expression);
            return me.Member.Name;
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
                return (MemberExpression)expression;

            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                var ue = (UnaryExpression)expression;
                return GetMemberExpression(ue.Operand);
            }

            throw new Exception(string.Format("Could not determine expression for member {0} of type {1}", expression, expression.NodeType));
        }
    }
}