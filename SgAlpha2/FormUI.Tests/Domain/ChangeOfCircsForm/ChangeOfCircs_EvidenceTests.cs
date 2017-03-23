using System.Text;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_EvidenceTests : DomainTest
    {
        [Test]
        public void AddEvidenceFile()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            var fileBytes = Encoding.ASCII.GetBytes("some content");
            form.AddEvidenceFile("filename1.txt", fileBytes);

            var storedForm = Repository.Load<ChangeOfCircs>(form.Id);

            storedForm.Evidence.Files.Count.Should().Be(1);

            var file = storedForm.Evidence.Files[0];
            file.Name.Should().Be("filename1.txt");
            //BlobStore.VerifyExists(file.Uri);
        }
    }
}
