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
    }
}
