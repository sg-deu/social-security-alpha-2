using System;
using System.Net;
using System.Runtime.Serialization;

namespace FormUI.Tests.Controllers.Util.Http
{
    [Serializable]
    public class UnexpectedStatusCodeException : Exception
    {
        private Response        _response;
        private HttpStatusCode _expectedStatusCode;

        public Response         Response            { get { return _response; } }
        public HttpStatusCode   ExpectedStatusCode  { get { return _expectedStatusCode; } }

        public UnexpectedStatusCodeException(Response response, HttpStatusCode expectedStatusCode)
            : base(FormatMessage(response, expectedStatusCode))
        {
            _response = response;
            _expectedStatusCode = expectedStatusCode;
        }

        private static string FormatMessage(Response response, HttpStatusCode expectedStatusCode)
        {
            return string.Format("Unexpected status code\nExpected {0} {1}\nActual {2} {3}",
                (int)expectedStatusCode,
                expectedStatusCode,
                response.StatusCode,
                response.StatusDescription);
        }

        protected UnexpectedStatusCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _response = (Response)info.GetValue("Response", typeof(Response));
            _expectedStatusCode = (HttpStatusCode)info.GetValue("ExpectedStatusCode", typeof(HttpStatusCode));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Response", _response);
            info.AddValue("ExpectedStatusCode", _expectedStatusCode);
            base.GetObjectData(info, context);
        }
    }
}
