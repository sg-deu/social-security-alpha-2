using System.Text;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class RemoveEvidenceFileTests : DomainTest
    {
        [Test]
        public void Execute_RemovesFile()
        {
            var existingForm = new BestStartGrantBuilder("form123").Insert();

            var cmdA = new AddEvidenceFile
            {
                FormId = "form123",
                Filename = "test.pdf",
                Content = Encoding.ASCII.GetBytes("evidence file"),
            };

            cmdA.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.Evidence.Files.Count.Should().Be(1);

            var cloudName = updatedForm.Evidence.Files[0].CloudName;

            var cmdR = new RemoveEvidenceFile
            {
                FormId = "form123",
                CloudName = cloudName,
            };

            cmdR.Execute();

            updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.Evidence.Files.Count.Should().Be(0);
        }
    }
}
