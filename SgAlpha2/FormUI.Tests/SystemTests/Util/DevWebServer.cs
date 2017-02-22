using System;
using System.Security.AccessControl;
using System.Threading;
using CassiniDev;

namespace FormUI.Tests.SystemTests.Util
{
    public static class DevWebServer
    {
        private static CassiniDevServer _server;
        private static Semaphore        _enforceSingleInstance;

        public static string RootUrl { get; private set; }

        public static void Start()
        {
            var security = new SemaphoreSecurity();
            security.AddAccessRule(new SemaphoreAccessRule("Everyone", SemaphoreRights.FullControl, AccessControlType.Allow));

            bool createdNew_notUsed;
            var semaphoreName = "Global\\DevWebServer";

            lock (typeof(DevWebServer))
                try
                {
                    DisposeSemaphore();

                    _enforceSingleInstance = new Semaphore(1, 1, semaphoreName, out createdNew_notUsed, security);

                    if (!_enforceSingleInstance.WaitOne(TimeSpan.FromSeconds(40)))
                        throw new Exception("Could not obtain semaphore: " + semaphoreName);
                }
                catch
                {
                    DisposeSemaphore();
                    throw;
                }

            _server = new CassiniDevServer();
            _server.StartServer(@"..\..\..\FormUI", 54078, "/", "localhost");

            RootUrl = _server.RootUrl;
        }

        public static void Stop()
        {
            DisposeSemaphore();

            if (_server != null)
                _server.StopServer();
        }

        private static void DisposeSemaphore()
        {
            lock (typeof(DevWebServer))
                if (_enforceSingleInstance != null)
                {
                    using (_enforceSingleInstance)
                        _enforceSingleInstance.Release();

                    _enforceSingleInstance = null;
                }
        }
    }
}
