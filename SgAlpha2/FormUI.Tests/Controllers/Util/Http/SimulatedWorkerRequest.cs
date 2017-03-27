using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace FormUI.Tests.Controllers.Util.Http
{
    public class SimulatedWorkerRequest : SimpleWorkerRequest
    {
        private Request                 _request;
        private HttpCookieCollection    _cookies;
        private int                     _statusCode;
        private string                  _statusDescription;

        public int      StatusCode          { get { return _statusCode; } }
        public string   StatusDescription   { get { return _statusDescription; } }

        public SimulatedWorkerRequest(Request request, HttpCookieCollection cookies, TextWriter output)
            : base(request.Url.TrimStart('~','/'), request.Query(), output)
        {
            _request = request;
            _cookies = cookies;
        }

        public override string GetHttpVerbName()
        {
            return _request.Verb;
        }

        public override string GetKnownRequestHeader(int index)
        {
            var headerName = GetKnownRequestHeaderName(index);

            if (headerName == "Cookie")
                return MakeCookieHeader();

            var header = _request.Headers[headerName];
            return header ?? base.GetKnownRequestHeader(index);
        }

        public override byte[] GetPreloadedEntityBody()
        {
            if (_request.FormValues == null && _request.FormFiles == null)
                return base.GetPreloadedEntityBody();

            if (_request.FormFiles != null)
                return MultipartForm();

            var sb = new StringBuilder();

            foreach (var formValue in _request.FormValues)
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(formValue.Name), HttpUtility.UrlEncode(formValue.Value));

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private byte[] MultipartForm()
        {
            using (var ms = new MemoryStream())
            {
                foreach (var formValue in _request.FormValues)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("--{0}", Request.Boundary);
                    sb.AppendFormat("\r\nContent-Disposition: form-data; name=\"{0}\"", formValue.Name);
                    sb.AppendFormat("\r\n\r\n{0}\r\n", formValue.Value);
                    var bytes = Encoding.ASCII.GetBytes(sb.ToString());
                    ms.Write(bytes, 0, bytes.Length);
                }

                foreach (var file in _request.FormFiles)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("--{0}", Request.Boundary);
                    sb.AppendFormat("\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "file1", file.FileName);
                    sb.AppendFormat("\r\nContent-Type: application/x-object\r\n\r\n");
                    var bytes = Encoding.ASCII.GetBytes(sb.ToString());
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Write(file.Content, 0, file.Content.Length);
                    bytes = Encoding.ASCII.GetBytes("\r\n");
                    ms.Write(bytes, 0, bytes.Length);
                }

                var final = string.Format("--{0}--", Request.Boundary);
                var finalBytes = Encoding.ASCII.GetBytes(final);
                ms.Write(finalBytes, 0, finalBytes.Length);

                return ms.ToArray();
            }
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            base.SendStatus(statusCode, statusDescription);
            _statusCode = statusCode;
            _statusDescription = statusDescription;
        }

        private string MakeCookieHeader()
        {
            if ((_cookies == null) || (_cookies.Count == 0))
                return null;

            var sb = new StringBuilder();

            foreach (string cookieName in _cookies)
                sb.AppendFormat("{0}={1};", cookieName, _cookies[cookieName].Value);

            return sb.ToString();
        }
    }
}
