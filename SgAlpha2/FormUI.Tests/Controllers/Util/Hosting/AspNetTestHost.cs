using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using FormUI.Tests.Controllers.Util.Http;

namespace FormUI.Tests.Controllers.Util.Hosting
{
    // borrowed from an excellent post on testing asp.net:
    // http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application/
    public class AspNetTestHost : IDisposable
    {
        public const string     RunningFlagFile = "MvcTestingAspNetTestHost.running.txt";

        private bool            _disposed;
        private Semaphore       _enforceSingleInstance;
        private AppDomainProxy  _appDomainProxy;

        public string PhysicalDirectory { get; protected set; }

        public AspNetTestHost(string physicalDirectory) : this(physicalDirectory, typeof(AppDomainProxy)) { }

        public AspNetTestHost(string physicalDirectory, Type appDomainProxyType) : this(physicalDirectory, "/", Timeout.InfiniteTimeSpan, appDomainProxyType) { }

        public AspNetTestHost(string physicalDirectory, string virtualDirectory, TimeSpan timeout, Type appDomainProxyType)
        {
            if (!typeof(AppDomainProxy).IsAssignableFrom(appDomainProxyType))
                throw new Exception(string.Format("Type: {0} should inherit from {1}", appDomainProxyType, typeof(AppDomainProxy)));

            PhysicalDirectory = Path.GetFullPath(physicalDirectory);

            var security = new SemaphoreSecurity();
            security.AddAccessRule(new SemaphoreAccessRule("Everyone", SemaphoreRights.FullControl, AccessControlType.Allow));

            bool createdNew_notUsed;
            var semaphoreName = "Global\\MvcTestingAspNetTestHost" + PhysicalDirectory.GetHashCode();
            _enforceSingleInstance = new Semaphore(1, 1, semaphoreName, out createdNew_notUsed, security);

            try
            {
                if (!_enforceSingleInstance.WaitOne(timeout))
                    throw new Exception("Could not obtain semaphore: " + semaphoreName);

                if (!Directory.Exists(PhysicalDirectory))
                    throw new Exception("Could not find directory: " + PhysicalDirectory);

                CopyTestBinaries();
                _appDomainProxy = (AppDomainProxy)ApplicationHost.CreateApplicationHost(appDomainProxyType, virtualDirectory, PhysicalDirectory);
                _appDomainProxy.RunCodeInAppDomain(InitHost);
            }
            catch
            {
                DeleteTestBinaries();
                using (_enforceSingleInstance)
                    _enforceSingleInstance.Release();
                throw;
            }
        }

        private static void InitHost()
        {
            CaptureResultFilter.Register();
        }

        public static AspNetTestHost For(string physicalDirectory)
        {
            return new AspNetTestHost(physicalDirectory);
        }

        public static AspNetTestHost For(string physicalDirectory, Type appDomainProxyType)
        {
            return new AspNetTestHost(physicalDirectory, appDomainProxyType);
        }

        public static AspNetTestHost For(string physicalDirectory, string virtualDirectory, TimeSpan timeout)
        {
            return new AspNetTestHost(physicalDirectory, virtualDirectory, timeout, typeof(AppDomainProxy));
        }

        public static AspNetTestHost For(string physicalDirectory, string virtualDirectory, TimeSpan timeout, Type appDomainProxyType)
        {
            return new AspNetTestHost(physicalDirectory, virtualDirectory, timeout, appDomainProxyType);
        }

        public void Test(Action<SimulatedHttpClient> testAction)
        {
            var serializableDelegate = new SerializableDelegate<Action<SimulatedHttpClient>>(testAction);
            _appDomainProxy.RunCodeInAppDomain(serializableDelegate, new ConsoleWriter());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AspNetTestHost()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (_appDomainProxy != null)
            {
                _appDomainProxy.RunCodeInAppDomain(() => HttpRuntime.UnloadAppDomain());
                _appDomainProxy = null;
            }

            DeleteTestBinaries();

            if (isDisposing && _enforceSingleInstance != null)
                using (_enforceSingleInstance)
                {
                    _enforceSingleInstance.Release();
                    _enforceSingleInstance = null;
                }

            _disposed = true;
        }

        private void CopyTestBinaries()
        {
            var webBinDir = Path.Combine(PhysicalDirectory, "bin");
            var testBinDir = AppDomain.CurrentDomain.BaseDirectory;
            var runningFlagFile = Path.Combine(webBinDir, RunningFlagFile);

            if (!Directory.Exists(webBinDir))
                throw new Exception("Could not find bin directory: " + webBinDir);

            if (File.Exists(runningFlagFile))
                DeleteTestBinaries();

            AppDomain.CurrentDomain.DomainUnload += (s, e) => Dispose();

            var sourceFiles = new List<string>();
            var destinationFiles = new List<string>();

            foreach (var srcFile in Directory.GetFiles(testBinDir))
            {
                var fileName = Path.GetFileName(srcFile);
                var destFile = Path.Combine(webBinDir, fileName);

                if (!File.Exists(destFile))
                {
                    sourceFiles.Add(srcFile);
                    destinationFiles.Add(destFile);
                }
            }

            File.WriteAllLines(runningFlagFile, destinationFiles);

            for (var i = 0; i < sourceFiles.Count; i++)
                File.Copy(sourceFiles[i], destinationFiles[i]);
        }

        private void DeleteTestBinaries()
        {
            var webBinDir = Path.Combine(PhysicalDirectory, "bin");
            var runningFlagFile = Path.Combine(webBinDir, RunningFlagFile);

            if (!File.Exists(runningFlagFile))
                return;

            var filesToDelete = File.ReadAllLines(runningFlagFile);

            foreach (var file in filesToDelete)
                if (File.Exists(file))
                    File.Delete(file);

            File.Delete(runningFlagFile);
        }
    }
}
