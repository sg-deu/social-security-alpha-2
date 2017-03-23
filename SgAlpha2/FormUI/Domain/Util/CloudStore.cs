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

        private static CloudBlobContainer InitContainer(bool clearContainer = false)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(_containerName);
            container.CreateIfNotExists();

            if (clearContainer)
                ClearContainer(container);

            return container;
        }

        private static void ClearContainer(CloudBlobContainer container)
        {
            if (container.Name != "unittest" && container.Name != "build")
                throw new Exception("Do not try to clear non-test container");

            var blobs = container.ListBlobs(useFlatBlobListing: true);
            var stripLength = container.Uri.ToString().Length + 1; // stip container name + trailing '/'

            foreach (var blob in blobs)
            {
                var blobName = blob.Uri.ToString().Substring(stripLength);
                container.GetBlobReference(blobName).Delete();
            }
        }

        public static CloudStore New()
        {
            return new CloudStore();
        }

        protected Lazy<CloudBlobContainer> _container;

        protected CloudStore(bool clearContainer = false)
        {
            _container = new Lazy<CloudBlobContainer>(() => InitContainer(clearContainer));
        }

        public void Store(string folder, string cloudFilename, string metadataFilename, byte[] content, string contentType = "application/octet-stream")
        {
            var fullName = $"{folder}/{cloudFilename}";
            var block = _container.Value.GetBlockBlobReference(fullName);

            if (block.Exists())
                throw new Exception($"{fullName} already exists");

            block.Properties.ContentType = contentType;
            block.Metadata.Add("filename", metadataFilename);
            block.UploadFromByteArray(content, 0, content.Length);
        }

        public IList<string> List(string folder)
        {
            var block = _container.Value.GetDirectoryReference(folder);
            var blobs = block.ListBlobs(useFlatBlobListing: true);
            var names = blobs.Select(b => b.Uri.Segments.Last());
            return names.ToList();
        }
    }
}