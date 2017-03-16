using System.Web.Mvc;
using FormUI.App_Start;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class TextArea : InputControl
    {
        private int? _maxLength;

        public TextArea(HtmlHelper helper, ControlContext controlContext) : base(helper, controlContext)
        {
        }

        public TextArea MaxLength(int? maxLength)
        {
            _maxLength = maxLength;
            return this;
        }

        protected override HtmlTag CreateTag()
        {
            var input = new HtmlTag("textarea")
                .Id(ControlContext.Id)
                .Name(ControlContext.Name)
                .TextIfEmpty(RenderValue())
                .AddClasses("form-control");

            var additionalValues = ControlContext.Metadata.AdditionalValues;
            var maxLength = _maxLength;

            if (!maxLength.HasValue && additionalValues.ContainsKey(Metadata.MaxLength))
                maxLength = (int)additionalValues[Metadata.MaxLength];

            if (maxLength.HasValue)
                input.Attr("maxlength", maxLength);

            return input;
        }
    }
}