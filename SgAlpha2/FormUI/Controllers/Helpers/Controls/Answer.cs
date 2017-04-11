using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class Answer : Control
    {
        public static Answer For<TModel, TProp>(HtmlHelper<TModel> helper, string labelText, Expression<Func<TModel, TProp>> property, Func<TProp, string> stringFunc, Func<TProp, string[]> classesFunc = null)
        {
            var propertyName = property.GetExpressionText();
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);
            var metaData = ModelMetadata.FromLambdaExpression(property, helper.ViewData);
            labelText = labelText ?? metaData.DisplayName ?? metaData.PropertyName;
            var modelValue = property.Compile()(helper.ViewData.Model);
            var value = stringFunc(modelValue);
            var classes = classesFunc?.Invoke(modelValue);
            return new Answer(helper, name, labelText, value, classes);
        }

        private string      _name;
        private string      _labelText;
        private string      _value;
        private string[]    _classes;
        private bool        _alwaysShow;

        public Answer(HtmlHelper helper, string name, string labelText, string value, string[] classes = null) : base(helper)
        {
            _name = name;
            _labelText = labelText;
            _value = value;
            _classes = classes;
        }

        public Answer AlwaysShow(bool alwaysShow = true)
        {
            _alwaysShow = alwaysShow;
            return this;
        }

        protected override HtmlTag CreateTag()
        {
            var container = new HtmlTag("container", t => t.NoTag());

            if (string.IsNullOrWhiteSpace(_value) && !_alwaysShow)
                return container;

            var dt = new HtmlTag("dt");
            var dd = new HtmlTag("dd");
            container.Append(dt).Append(dd);

            dt.Text(_labelText);
            dd.Attr("data-answer-for", _name).Text(_value);

            if (_classes != null)
                dd.AddClasses(_classes);

            return container;
        }
    }
}