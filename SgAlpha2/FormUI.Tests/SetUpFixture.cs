using System;
using System.Security.AccessControl;
using System.Threading;
using FormUI.Tests.Domain.Util;
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
            Console.WriteLine("Aquiring machine-wide lock ... ");

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

                    if (!_enforceSingleInstance.WaitOne(TimeSpan.FromSeconds(240)))
                        throw new Exception("Could not obtain semaphore: " + semaphoreName);
                }

                Console.WriteLine("Aquired");

                LocalRepository.VerifyRunning();
            }
            catch
            {
                TearDown();
                throw;
            }

            TestRegistry.TestHasFailed = false;
        }

        [TearDown]
        public void TearDown()
        {
            LocalRepository.AddTestDocument();
            DisposeSemaphore();
        }

        private static void DisposeSemaphore()
        {
            lock (typeof(SetUpFixture))
                if (_enforceSingleInstance != null)
                {
                    try
                    {
                        using (_enforceSingleInstance)
                            _enforceSingleInstance.Release();
                    }
                    catch { }
                    finally
                    {
                        _enforceSingleInstance = null;
                        Console.WriteLine("Released machine-wide lock");
                    }
                }
        }
    }
}
