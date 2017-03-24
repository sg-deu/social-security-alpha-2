using System.Text;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddEvidenceFileTests : DomainTest
    {
        [Test]
        public void Execute_AddsFile()
        {
            var existingForm = new ChangeOfCircsBuilder("form123").Insert();

            var cmd = new AddEvidenceFile
            {
                FormId = "form123",
                Filename = "test.pdf",
                Content = Encoding.ASCII.GetBytes("evidence file"),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Evidence.Files.Count.Should().Be(1);
        }
    }
}
