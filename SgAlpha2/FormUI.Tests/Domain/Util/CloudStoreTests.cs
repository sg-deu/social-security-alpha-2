using System;
using System.Text;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    [Explicit("experimenting with Azure Blob storage")]
    public class CloudStoreTests : DomainTest
    {
        private static string _guidFolder = Guid.NewGuid().ToString();

        [Test]
        public void Store()
        {
            CloudStore.Init("UseDevelopmentStorage=true;", "unittest");

            var content = Encoding.ASCII.GetBytes("my file contents");

            CloudStore.Store("folder1", "file1.txt", content);
            CloudStore.Store("folder1", "file2.txt", content);
            CloudStore.Store("folder1", "file3.txt", content);
            CloudStore.Store("folder2", "file1.txt", content);
            CloudStore.Store("folder2", "file2.txt", content);
            CloudStore.Store("folder2", "file3.txt", content);
            CloudStore.Store(_guidFolder, "file1.txt", content);
            CloudStore.Store(_guidFolder, "file2.txt", content);
            CloudStore.Store(_guidFolder, "file3.txt", content);
        }

        [Test]
        public void ListFiles()
        {
            CloudStore.Init("UseDevelopmentStorage=true;", "unittest");

            var files = CloudStore.List("folder1");

            foreach (var file in files)
                Console.WriteLine(file);
        }

        [Test]
        public void DeleteFiles()
        {
            CloudStore.Init("UseDevelopmentStorage=true;", "unittest");
            CloudStore.DeleteUnitTestContainer();
            Console.WriteLine(_guidFolder);
        }
    }
}
