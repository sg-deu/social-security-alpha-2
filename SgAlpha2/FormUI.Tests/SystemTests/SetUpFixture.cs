using System;
using CassiniDev;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private static CassiniDevServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new CassiniDevServer();
            _server.StartServer(@"..\..\..\FormUI", 54077, "/", "localhost");
            Console.WriteLine(_server.RootUrl);
        }

        [TearDown]
        public void TearDown()
        {
            _server.StopServer();
        }
    }
}
