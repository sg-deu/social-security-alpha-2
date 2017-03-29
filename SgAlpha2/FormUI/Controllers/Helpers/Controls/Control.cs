using System;
using System.Web;
using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class Control : IHtmlString
    {
        private HtmlHelper      _helper;
        private Lazy<UrlHelper> _urlHelper;
        private Action<HtmlTag> _tagMutator;

        public Control(HtmlHelper helper)
        {
            _helper = helper;
            _urlHelper = new Lazy<UrlHelper>(() => new UrlHelper(_helper.ViewContext.RequestContext));
        }

        protected UrlHelper     UrlHelper   { get { return _urlHelper.Value; } }

        public string ToHtmlString()
        {
            return GenerateTag().ToHtmlString();
        }

        public Control Tag(Action<HtmlTag> tagMutator)
        {
            _tagMutator = tagMutator;
            return this;
        }

        public HtmlTag GenerateTag(bool inlineMandatory = false)
        {
            var tag = CreateTag(inlineMandatory);
            _tagMutator?.Invoke(tag);
            return tag;
        }

        public virtual bool HandlesMandatoryInline() { return false; }

        protected static HtmlTag NewMandatory()
        {
            return new HtmlTag("span").AddClasses("required-icon").Text("*");
        }

        protected virtual HtmlTag CreateTag(bool inlineMandatory = false)
        {
            if (HandlesMandatoryInline() && inlineMandatory)
                throw new Exception("override CreateTag(bool) to handle inline-mandatory for " + GetType());

            return CreateTag();
        }

        protected virtual HtmlTag CreateTag() { return new HtmlTag("span"); }
    }
}