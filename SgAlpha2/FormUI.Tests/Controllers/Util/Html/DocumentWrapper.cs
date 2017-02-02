using AngleSharp.Dom.Html;

namespace FormUI.Tests.Controllers.Util.Html
{
    public class DocumentWrapper : ParentNodeWrapper
    {
        private IHtmlDocument   _document;
        private string          _requestUrl;

        public DocumentWrapper(IHtmlDocument document, string requestUrl) : base(document)
        {
            _document = document;
            _requestUrl = requestUrl;
        }

        public IHtmlDocument Document { get { return _document; } }

        public TypedForm<T> Form<T>()                   { return FormHelper.Scrape<T>(this, _requestUrl); }
        public TypedForm<T> Form<T>(int index)          { return FormHelper.Scrape<T>(this, _requestUrl, index); }
        public TypedForm<T> Form<T>(string cssSelector) { return FormHelper.Scrape<T>(this, _requestUrl, cssSelector); }
    }
}
