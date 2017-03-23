using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FormUI.Domain.Util
{
    public class CloudStore : ICloudStore
    {
        private static string _connectionString;
        private static string _containerName;

        public static void Init(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            InitContainer();
        }

        private static CloudBlobContainer InitContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(_containerName);
            container.CreateIfNotExists();
            return container;
        }

        public static CloudStore New()
        {
            return new CloudStore();
        }

        protected Lazy<CloudBlobContainer> _container;

        protected CloudStore()
        {
            _container = new Lazy<CloudBlobContainer>(InitContainer);
        }

        public void Store(string folder, string filename, byte[] content)
        {
            var fullName = $"{folder}/{filename}";
            var block = _container.Value.GetBlockBlobReference(fullName);

            if (block.Exists())
                throw new Exception($"{fullName} already exists");

            block.UploadFromByteArray(content, 0, content.Length);
        }

        public IList<string> List(string folder)
        {
            var block = _container.Value.GetDirectoryReference(folder);
            var blobs = block.ListBlobs(useFlatBlobListing: true);
            var names = blobs.Select(b => b.Uri.Segments.Last());
            return names.ToList();
        }

        protected void ClearContainer()
        {
            _container.Value.DeleteIfExists();
            _container.Value.CreateIfNotExists();
        }
    }
}