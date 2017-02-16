using System;
using FormUI.Tests.SystemTests.Util;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("SetUp SystemTests");

            try
            {
                DevWebServer.Start();
            }
            catch
            {
                TearDown();
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown SystemTests");
            DevWebServer.Stop();
        }
    }
}
