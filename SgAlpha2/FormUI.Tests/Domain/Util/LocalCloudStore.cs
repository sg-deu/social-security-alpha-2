using System.Net.Sockets;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    public class LocalCloudStore : CloudStore
    {
        private const string DefaultStorageConnectionString = "UseDevelopmentStorage=true;";
        private const string DefaultStorageName             = "unittest";

        private static bool _isSetup = false;

        private static void SetUp()
        {
            if (_isSetup)
                return;

            var storageConnectionString = VstsSettings.GetSetting("storageConnectionString", DefaultStorageConnectionString);
            var storageName = VstsSettings.GetSetting("storageName", DefaultStorageName);

            CloudStore.Init(storageConnectionString, storageName);
            _isSetup = true;
        }

        public static void VerifyRunning()
        {
            var storageConnectionString = VstsSettings.GetSetting("storageConnectionString", DefaultStorageConnectionString);

            if (storageConnectionString != DefaultStorageConnectionString)
                return; // this is just a check to help local devs

            try
            {
                using (var tcpClient = new TcpClient())
                    tcpClient.Connect("localhost", 10000);
            }
            catch
            {
                Assert.Fail("Could not verify Azure Blob Storage emulator connection - please verify Storage Emulator is running");
            }
        }

        public static LocalCloudStore New(bool clearContainer = true)
        {
            SetUp();
            return new LocalCloudStore(clearContainer);
        }

        private LocalCloudStore(bool clearContainer) : base(clearContainer)
        {
        }

        public LocalCloudStore Register()
        {
            DomainRegistry.CloudStore = this;
            return this;
        }
    }
}
