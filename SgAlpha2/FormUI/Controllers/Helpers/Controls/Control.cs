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

        protected HtmlHelper    Helper      { get { return _helper; } }
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

        public HtmlTag GenerateTag()
        {
            var tag = CreateTag();
            _tagMutator?.Invoke(tag);
            return tag;
        }

        protected abstract HtmlTag CreateTag();
    }
}