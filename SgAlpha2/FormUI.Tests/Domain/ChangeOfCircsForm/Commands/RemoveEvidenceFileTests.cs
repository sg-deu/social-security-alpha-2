using System.Text;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class RemoveEvidenceFileTests : DomainTest
    {
        [Test]
        public void Execute_RemovesFile()
        {
            var existingForm = new ChangeOfCircsBuilder("form123").Insert();

            var cmdA = new AddEvidenceFile
            {
                FormId = "form123",
                Filename = "test.pdf",
                Content = Encoding.ASCII.GetBytes("evidence file"),
            };

            cmdA.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Evidence.Files.Count.Should().Be(1);

            var cloudName = updatedForm.Evidence.Files[0].CloudName;

            var cmdR = new RemoveEvidenceFile
            {
                FormId = "form123",
                CloudName = cloudName,
            };

            cmdR.Execute();

            updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Evidence.Files.Count.Should().Be(0);
        }
    }
}
