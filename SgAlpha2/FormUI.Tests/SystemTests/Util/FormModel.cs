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

        public void TypeDate(Expression<Func<T, object>> property, string dayText, string monthText, string yearText)
        {
            var name = property.GetExpressionText();
            _app.TypeText(name + "_day", dayText);
            _app.TypeText(name + "_month", monthText);
            _app.TypeText(name + "_year", yearText);
        }

        public void SelectRadio<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var name = property.GetExpressionText();
            var valueText = value.ToString();
            _app.SelectRadio(name, valueText);
        }
    }
}
