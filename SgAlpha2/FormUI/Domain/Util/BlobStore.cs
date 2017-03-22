using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FormUI.Domain.Util
{
    public class BlobStore
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

        public static void Store(string filename, byte[] content)
        {
            var block = _container.GetBlockBlobReference(filename);
            block.UploadFromByteArray(content, 0, content.Length);
        }
    }
}