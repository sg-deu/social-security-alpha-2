using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace FormUI.Tests.SystemTests.Util
{
    [TestFixture]
    public abstract class SystemTest
    {
        protected BrowserApp App { get; set; }

        [SetUp]
        protected virtual void SetUp()
        {
            var runHeadless = RunHeadless();
            App = new BrowserApp(runHeadless);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            using (App) { }
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
