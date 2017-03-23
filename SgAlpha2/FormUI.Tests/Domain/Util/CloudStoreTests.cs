using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public class CloudStoreTests : DomainTest
    {
        private static string _guidFolder = Guid.NewGuid().ToString();

        [Test]
        public void Store()
        {
            var content = Encoding.ASCII.GetBytes("my file contents");

            CloudStore.Store("folder1", "file1.txt", "metadata_filename", content);
            CloudStore.Store("folder1", "file2.txt", "metadata_filename", content);
            CloudStore.Store("folder1", "file3.txt", "metadata_filename", content);
            CloudStore.Store("folder2", "file1.txt", "metadata_filename", content);
            CloudStore.Store("folder2", "file2.txt", "metadata_filename", content);
            CloudStore.Store("folder2", "file3.txt", "metadata_filename", content);
            CloudStore.Store(_guidFolder, "file1.txt", "metadata_filename", content);
            CloudStore.Store(_guidFolder, "file2.txt", "metadata_filename", content);
            CloudStore.Store(_guidFolder, "file3.txt", "metadata_filename", content);

            CloudStore.List("folder1").Count.Should().Be(3);
        }
    }
}
