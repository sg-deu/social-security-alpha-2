using System;
using System.Linq.Expressions;
using FormUI.Controllers.Helpers;

namespace FormUI.Tests.SystemTests.Util
{
    public class FormModel<T>
    {
        private static BrowserApp _app;

        public FormModel(BrowserApp app)
        {
            _app = app;
        }

        public void TypeText(Expression<Func<T, object>> property, string text)
        {
            var name = property.GetExpressionText();
            _app.TypeText(name, text);
        }

        public void GetText(string testText, Expression<Func<T, object>> property, Action<string> verify)
        {
            var name = property.GetExpressionText();
            _app.GetText(testText, name, verify);
        }

        public void TypeDate(Expression<Func<T, object>> property, string dayText, string monthText, string yearText)
        {
            var name = property.GetExpressionText();
            _app.TypeText(name + "_day", dayText);
            _app.TypeText(name + "_month", monthText);
            _app.TypeText(name + "_year", yearText);
        }

        public void TypeDate(Expression<Func<T, object>> property, DateTime date)
        {
            var dayText = date.Day.ToString();
            var monthText = date.Month.ToString();
            var yearText = date.Year.ToString();
            TypeDate(property, dayText, monthText, yearText);
        }

        public void BlurDate(Expression<Func<T, object>> property)
        {
            var name = property.GetExpressionText();
            _app.Blur(name + "_year");
        }

        public void SelectRadio<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var name = property.GetExpressionText();
            var valueText = value.ToString();
            _app.SelectRadio(name, valueText);
        }

        public void Check(Expression<Func<T, bool>> property, bool check)
        {
            var name = property.GetExpressionText();
            _app.Check(name, check);
        }

        public string CssSelectFormGroup(Expression<Func<T, object>> property)
        {
            var name = property.GetExpressionText();
            return "#" + name + "_FormGroup";
        }
    }
}
