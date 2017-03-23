using System;
using System.Text;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    [Explicit("experimenting with Azure Blob storage")]
    public class BlobStoreTests : DomainTest
    {
        private static string _guidFolder = Guid.NewGuid().ToString();

        [Test]
        public void Store()
        {
            BlobStore.Init("UseDevelopmentStorage=true;", "unittest");

            var content = Encoding.ASCII.GetBytes("my file contents");

            BlobStore.Store("folder1", "file1.txt", content);
            BlobStore.Store("folder1", "file2.txt", content);
            BlobStore.Store("folder1", "file3.txt", content);
            BlobStore.Store("folder2", "file1.txt", content);
            BlobStore.Store("folder2", "file2.txt", content);
            BlobStore.Store("folder2", "file3.txt", content);
            BlobStore.Store(_guidFolder, "file1.txt", content);
            BlobStore.Store(_guidFolder, "file2.txt", content);
            BlobStore.Store(_guidFolder, "file3.txt", content);
        }

        [Test]
        public void ListFiles()
        {
            BlobStore.Init("UseDevelopmentStorage=true;", "unittest");

            var files = BlobStore.List("folder1");

            foreach (var file in files)
                Console.WriteLine(file);
        }

        [Test]
        public void DeleteFiles()
        {
            BlobStore.Init("UseDevelopmentStorage=true;", "unittest");
            BlobStore.DeleteUnitTestContainer();
            Console.WriteLine(_guidFolder);
        }
    }
}
