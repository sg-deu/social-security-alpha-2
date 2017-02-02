using System;
using System.IO;
using System.Web;
using FormUI.Tests.Controllers.Util.Hosting;

namespace FormUI.Tests.Controllers.Util.Http
{
    public interface ISimulatedHttpClient
    {
        Response Process(Request request, Action<Request> modifier = null);
    }

    [Serializable]
    public class SimulatedHttpClient : ISimulatedHttpClient
    {
        public static string LastResponseText { get; protected set; }

        private bool _expectedError;

        public ConsoleWriter        ConsoleWriter   { get; protected set; }
        public HttpCookieCollection Cookies         { get; protected set; }

        public SimulatedHttpClient(ConsoleWriter consoleWriter)
        {
            ConsoleWriter = consoleWriter;
            Cookies = new HttpCookieCollection();
        }

        public void ExpectError()
        {
            _expectedError = true;
        }

        public bool HadExpectedError()
        {
            var hadExpectedError = _expectedError;
            _expectedError = false;
            return hadExpectedError;
        }

        public Response Get(string url, Action<Request> modifier = null)
        {
            var request = new Request(url, "GET");
            return Process(request, modifier);
        }

        public Response Post(string url, Action<Request> modifier = null)
        {
            var request = new Request(url, "POST");
            return Process(request, modifier);
        }

        public Response Process(Request request, Action<Request> modifier = null)
        {
            if (modifier != null)
                modifier(request);

            using (var output = new StringWriter())
            {
                CaptureResultFilter.LastResult = null;

                var workerRequest = new SimulatedWorkerRequest(request, Cookies, output);
                HttpRuntime.ProcessRequest(workerRequest);

                var responseText = output.ToString();
                LastResponseText = responseText;

                var response = new Response
                {
                    StatusCode          = workerRequest.StatusCode,
                    StatusDescription   = workerRequest.StatusDescription,
                    Text                = responseText,
                    LastResult          = CaptureResultFilter.LastResult,
                };

                if (request.ExptectedResponse.HasValue && request.ExptectedResponse.Value != response.HttpStatusCode)
                    throw new UnexpectedStatusCodeException(response, request.ExptectedResponse.Value);

                return response;
            }
        }
    }
}
