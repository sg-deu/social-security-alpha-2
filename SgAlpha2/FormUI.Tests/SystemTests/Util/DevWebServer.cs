using System.Security.AccessControl;
using CassiniDev;

namespace FormUI.Tests.SystemTests.Util
{
    public static class DevWebServer
    {
        private static CassiniDevServer _server;

        public static string RootUrl { get; private set; }

        public static void Start()
        {
            var security = new SemaphoreSecurity();
            security.AddAccessRule(new SemaphoreAccessRule("Everyone", SemaphoreRights.FullControl, AccessControlType.Allow));

            _server = new CassiniDevServer();
            _server.StartServer(@"..\..\..\FormUI", 54078, "/", "localhost");

            RootUrl = _server.RootUrl;
        }

        public static void Stop()
        {
            if (_server != null)
                _server.StopServer();
        }
    }
}
