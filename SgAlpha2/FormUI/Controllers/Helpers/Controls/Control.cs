using System;
using System.Web;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class Control : IHtmlString
    {
        private Action<HtmlTag> _tagMutator;

        public string ToHtmlString()
        {
            return GenerateTag().ToHtmlString();
        }

        public Control Tag(Action<HtmlTag> tagMutator)
        {
            _tagMutator = tagMutator;
            return this;
        }

        public HtmlTag GenerateTag()
        {
            var tag = CreateTag();
            _tagMutator?.Invoke(tag);
            return tag;
        }

        protected abstract HtmlTag CreateTag();
    }
}