using System;
using System.Security.AccessControl;
using System.Threading;
using NUnit.Framework;

namespace FormUI.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private static Semaphore _enforceSingleInstance;

        [SetUp]
        public void SetUp()
        {
            Console.Write("Waiting for existing test run to complete ... ");

            try
            {
                var security = new SemaphoreSecurity();
                security.AddAccessRule(new SemaphoreAccessRule("Everyone", SemaphoreRights.FullControl, AccessControlType.Allow));

                bool createdNew_notUsed;
                var semaphoreName = "Global\\FormUITests";

                lock (typeof(SetUpFixture))
                {
                    DisposeSemaphore();

                    _enforceSingleInstance = new Semaphore(1, 1, semaphoreName, out createdNew_notUsed, security);

                    if (!_enforceSingleInstance.WaitOne(TimeSpan.FromSeconds(180)))
                        throw new Exception("Could not obtain semaphore: " + semaphoreName);
                }

                Console.WriteLine("existing tests complete");
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
            DisposeSemaphore();
        }

        private static void DisposeSemaphore()
        {
            lock (typeof(SetUpFixture))
                if (_enforceSingleInstance != null)
                {
                    using (_enforceSingleInstance)
                        _enforceSingleInstance.Release();

                    _enforceSingleInstance = null;
                }
        }
    }
}
