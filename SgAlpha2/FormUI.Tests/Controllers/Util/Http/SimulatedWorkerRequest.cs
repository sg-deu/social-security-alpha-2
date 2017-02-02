using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace FormUI.Tests.Controllers.Util.Http
{
    public class SimulatedWorkerRequest : SimpleWorkerRequest
    {
        private Request _request;
        private int     _statusCode;
        private string  _statusDescription;

        public int      StatusCode          { get { return _statusCode; } }
        public string   StatusDescription   { get { return _statusDescription; } }

        public SimulatedWorkerRequest(Request request, TextWriter output)
            : base(request.Url.TrimStart('~','/'), request.Query(), output)
        {
            _request = request;
        }

        public override string GetHttpVerbName()
        {
            return _request.Verb;
        }

        public override string GetKnownRequestHeader(int index)
        {
            var headerName = GetKnownRequestHeaderName(index);
            var header = _request.Headers[headerName];
            return header ?? base.GetKnownRequestHeader(index);
        }

        public override byte[] GetPreloadedEntityBody()
        {
            if (_request.FormValues == null)
                return base.GetPreloadedEntityBody();

            var sb = new StringBuilder();

            foreach (var formValue in _request.FormValues)
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(formValue.Name), HttpUtility.UrlEncode(formValue.Value));

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            base.SendStatus(statusCode, statusDescription);
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }
    }
}
