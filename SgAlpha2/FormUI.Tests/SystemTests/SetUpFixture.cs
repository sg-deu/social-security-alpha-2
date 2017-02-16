using System;
using System.Diagnostics;
using System.Linq;
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
            var runHeadless = RunHeadless();

            try
            {
                DevWebServer.Start();
                BrowserApp.Start(runHeadless);
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
            BrowserApp.Stop();
        }

        private bool RunHeadless()
        {
            var allProcesses = Process.GetProcesses();
            var processNames = allProcesses.Select(p => p.ProcessName).ToList();

            var nunitGuiRunning = processNames.Contains("nunit-x86");
            var nunitConsoleRunning = processNames.Contains("nunit-console-x86");

            if (nunitGuiRunning && !nunitConsoleRunning)
            {
                Console.WriteLine("Detected nunit-x86.exe (i.e., NUnitGui); running system tests using Chrome");
                return false;
            }
            else
            {
                Console.WriteLine("Could not find nunit-x86.exe (i.e., NUnitGui); running system tests headless using PhantomJS");
                return true;
            }
        }
    }
}
