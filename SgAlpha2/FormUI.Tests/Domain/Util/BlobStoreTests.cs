using System.Text;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    [Explicit("experimenting with Azure Blob storage")]
    public class BlobStoreTests : DomainTest
    {
        [Test]
        public void Store()
        {
            BlobStore.Init("UseDevelopmentStorage=true;", "unittest");

            var content = Encoding.ASCII.GetBytes("my file contents");

            BlobStore.Store("file.txt", content);
        }
    }
}
