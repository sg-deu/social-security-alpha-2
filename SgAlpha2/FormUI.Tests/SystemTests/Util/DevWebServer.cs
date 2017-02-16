using CassiniDev;

namespace FormUI.Tests.SystemTests.Util
{
    public static class DevWebServer
    {
        private static CassiniDevServer _server;

        public static string RootUrl { get; private set; }

        public static void Start()
        {
            _server = new CassiniDevServer();
            _server.StartServer(@"..\..\..\FormUI", 54078, "/", "localhost");
            RootUrl = _server.RootUrl;
        }

        public static void Stop()
        {
            _server.StopServer();
        }
    }
}
