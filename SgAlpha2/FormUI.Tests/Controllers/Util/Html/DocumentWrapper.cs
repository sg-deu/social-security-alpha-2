using AngleSharp.Dom.Html;

namespace FormUI.Tests.Controllers.Util.Html
{
    public class DocumentWrapper : ParentNodeWrapper
    {
        private IHtmlDocument _document;

        public DocumentWrapper(IHtmlDocument document) : base(document)
        {
            _document = document;
        }

        public IHtmlDocument Document { get { return _document; } }

        public TypedForm<T> Form<T>()                   { return FormHelper.Scrape<T>(this); }
        public TypedForm<T> Form<T>(int index)          { return FormHelper.Scrape<T>(this, index); }
        public TypedForm<T> Form<T>(string cssSelector) { return FormHelper.Scrape<T>(this, cssSelector); }
    }
}
