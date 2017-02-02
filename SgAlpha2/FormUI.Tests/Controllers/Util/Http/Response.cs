using System;
using System.Net;
using System.Web.Mvc;
using AngleSharp.Parser.Html;
using FormUI.Tests.Controllers.Util.Html;

namespace FormUI.Tests.Controllers.Util.Http
{
    [Serializable]
    public class Response
    {
        public static Func<HtmlParser> NewParser = () => new HtmlParser();

        [NonSerialized]
        private Lazy<DocumentWrapper> _documentWrapper;

        public Response()
        {
            _documentWrapper = new Lazy<DocumentWrapper>(() =>
            {
                var parser = NewParser();
                var doc = parser.Parse(Text);
                return new DocumentWrapper(doc);
            });

        }

        public int              StatusCode;
        public string           StatusDescription;
        public string           Text;

        [NonSerialized]
        public ResultExecutedContext LastResult;

        public HttpStatusCode   HttpStatusCode  { get { return (HttpStatusCode)StatusCode; } }
        public DocumentWrapper  Doc             { get { return _documentWrapper.Value; } }

        public TypedForm<T> Form<T>()                   { return Doc.Form<T>(); }
        public TypedForm<T> Form<T>(int index)          { return Doc.Form<T>(index); }
        public TypedForm<T> Form<T>(string cssSelector) { return Doc.Form<T>(cssSelector); }

        public ActionResult ActionResult()
        {
            if (LastResult == null)
                throw new Exception("Expected ActionResult, but got no result from global filter CaptureResultFilter");

            return LastResult.Result;
        }

        public T ActionResultOf<T>() where T : ActionResult
        {
            var actionResult = ActionResult();

            if (actionResult == null)
                throw new Exception("Expected ActionResult, but got <null>");

            var result = (actionResult as T);

            if (result == null)
                throw new Exception(string.Format("Expected {0}, but got {1}", typeof(T).Name, actionResult.GetType().Name));

            return result;
        }
    }
}
