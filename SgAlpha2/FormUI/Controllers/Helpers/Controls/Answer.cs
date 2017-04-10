using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class Answer : Control
    {
        public static Answer For<TModel, TProp>(HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProp>> property, Func<TProp, string> stringValue)
        {
            var propertyName = property.GetExpressionText();
            var metaData = ModelMetadata.FromLambdaExpression(property, helper.ViewData);
            labelText = labelText ?? metaData.DisplayName ?? metaData.PropertyName;
            var modelValue = property.Compile()(helper.ViewData.Model);
            var value = stringValue(modelValue);
            return new Answer(helper, propertyName, labelText, value);
        }

        private string _name;
        private string _labelText;
        private string _value;

        public Answer(HtmlHelper helper, string name, string labelText, string value) : base(helper)
        {
            _name = name;
            _labelText = labelText;
            _value = value;
        }

        protected override HtmlTag CreateTag()
        {
            var dt = new HtmlTag("dt");
            var dd = new HtmlTag("dd");
            var container = new HtmlTag("container", t => t.NoTag()).Append(dt).Append(dd);

            dt.Text(_labelText);
            dd.Attr("data-answer-for", _name).Text(_value);

            return container;
        }
    }
}