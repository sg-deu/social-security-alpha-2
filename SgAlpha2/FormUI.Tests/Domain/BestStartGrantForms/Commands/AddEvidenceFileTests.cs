using System.Text;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddEvidenceFileTests : DomainTest
    {
        [Test]
        [Ignore]
        public void Execute_AddsFile()
        {
            //var existingForm = new BestStartGrantBuilder("form123").Insert();

            //var cmd = new AddEvidenceFile
            //{
            //    FormId = "form123",
            //    Filename = "test.pdf",
            //    Content = Encoding.ASCII.GetBytes("evidence file"),
            //};

            //cmd.Execute();

            //var updatedForm = Repository.Load<BestStartGrant>("form123");
            //updatedForm.Evidence.Files.Count.Should().Be(1);
        }
    }
}
