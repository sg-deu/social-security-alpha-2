using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FormUI.Domain.Util
{
    public class CloudStore
    {
        private static CloudBlobClient      _blobClient;
        private static CloudBlobContainer   _container;

        public static void Init(string connectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();

            _container = _blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists();
        }

        public static void Store(string folder, string filename, byte[] content)
        {
            var fullName = $"{folder}/{filename}";
            var block = _container.GetBlockBlobReference(fullName);

            if (block.Exists())
                throw new System.Exception($"{fullName} already exists");

            block.UploadFromByteArray(content, 0, content.Length);
        }

        public static IList<string> List(string folder)
        {
            var block = _container.GetDirectoryReference(folder);
            var blobs = block.ListBlobs(useFlatBlobListing: true);
            var names = blobs.Select(b => b.Uri.Segments.Last());
            return names.ToList();
        }

        public static void DeleteUnitTestContainer()
        {
            _container.DeleteIfExists();
        }
    }
}