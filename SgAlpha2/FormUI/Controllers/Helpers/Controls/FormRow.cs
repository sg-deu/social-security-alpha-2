using System;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public enum ControlWidth
    {
        Small,
        Medium,
        Max,
    }

    public class FormRow<TControl> : Control
        where TControl : Control
    {
        private string          _id;
        private string          _labelText;
        private TControl        _control;
        private string          _hintHtml;
        private bool            _mandatory;
        private string          _beforeControlHtml;
        private ControlWidth?   _controlWidth;
        private bool            _initiallyHidden;
        private string          _ajaxOnChangeAction;

        public FormRow(HtmlHelper helper, string id, string labelText, TControl control) : base(helper)
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

        public FormRow<TControl> Mandatory(bool mandatory = true)
        {
            _mandatory = mandatory;
            return this;
        }

        public FormRow<TControl> BeforeControl(string beforeControlHtml)
        {
            _beforeControlHtml = beforeControlHtml;
            return this;
        }

        public FormRow<TControl> Width(ControlWidth? controlWidth)
        {
            _controlWidth = controlWidth;
            return this;
        }

        public FormRow<TControl> InitiallyHidden(bool initiallyHidden = true)
        {
            _initiallyHidden = initiallyHidden;
            return this;
        }

        public FormRow<TControl> AjaxOnChange(string ajaxAction)
        {
            _ajaxOnChangeAction = ajaxAction;
            return this;
        }

        public FormRow<TControl> Control(Action<TControl> controlMutator)
        {
            controlMutator(_control);
            return this;
        }

        protected override HtmlTag CreateTag()
        {
            var label = new HtmlTag("label").Attr("for", _id);

            if (_mandatory && !_control.HandlesMandatoryInline())
                label.Append(NewMandatory());

            label.Append(new HtmlTag("span").Text(_labelText));

            var controlTag = _control.GenerateTag(_mandatory);
            var inputWrapper = new DivTag().AddClasses("input-wrapper").Append(controlTag);

            var formGroup = new DivTag()
                .Id(_id + "_FormGroup")
                .AddClasses("form-group")
                .Append(label)
                .Append(inputWrapper);

            if (!string.IsNullOrWhiteSpace(_hintHtml))
            {
                var hint = new HtmlTag("p").AddClasses("help-block").AppendHtml(_hintHtml);
                label.Append(hint);
            }

            if (!string.IsNullOrWhiteSpace(_beforeControlHtml))
            {
                var beforeControl = new HtmlTag("span").AppendHtml(_beforeControlHtml);
                label.Append(beforeControl);
            }

            if (_controlWidth.HasValue && _controlWidth.Value != ControlWidth.Max)
                inputWrapper.AddClass("control-width-" + _controlWidth.Value.ToString().ToLower());

            if (_initiallyHidden)
                formGroup.AddClasses("initially-hidden");

            if (!string.IsNullOrWhiteSpace(_ajaxOnChangeAction))
                formGroup.Attr("data-ajax-change", UrlHelper.Content(_ajaxOnChangeAction));

            return formGroup;
        }
    }
}