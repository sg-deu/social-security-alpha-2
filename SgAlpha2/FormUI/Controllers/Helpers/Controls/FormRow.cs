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

        public FormRow<TControl> Label(string labelText)
        {
            _labelText = labelText;
            return this;
        }

        public FormRow<TControl> Hint(string hintHtml)
        {
            _hintHtml = hintHtml;
            return this;
        }

        public FormRow<TControl> Control(Action<TControl> controlMutator)
        {
            controlMutator(_control);
            return this;
        }

        protected override HtmlTag CreateTag()
        {
            var label = new HtmlTag("label").Text(_labelText).Attr("for", _id);

            var controlTag = _control.GenerateTag();
            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(controlTag);

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