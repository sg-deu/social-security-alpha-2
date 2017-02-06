using System;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class FormRow<TControl> : Control
        where TControl : Control
    {
        private string      _id;
        private string      _labelText;
        private TControl    _control;
        private string      _hintHtml;

        public FormRow(string id, string labelText, TControl control)
        {
            _id = id;
            _labelText = labelText;
            _control = control;
        }

        public FormRow<TControl> Hint(string hintHtml)
        {
            _hintHtml = hintHtml;
            return this;
        }

        public FormRow<TControl> With(Action<TControl> mutator)
        {
            mutator(_control);
            return this;
        }

        public override HtmlTag ToTag()
        {
            var label = new HtmlTag("label").Text(_labelText).Attr("for", _id);

            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(_control.ToTag());

            var formGroup = new DivTag()
                .AddClasses("form-group")
                .Append(label)
                .Append(inputWrapper);

            if (!string.IsNullOrWhiteSpace(_hintHtml))
            {
                var hint = new HtmlTag("p").AddClasses("help-block").AppendHtml(_hintHtml);
                label.Append(hint);
            }

            return formGroup;
        }
    }
}