using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddEvidenceTests : DomainTest
    {
        [Test]
        [Ignore]
        public void Execute_StoresEvidence()
        {
            //var existingForm = new BestStartGrantBuilder("form123")
            //    .With(f => f.Evidence, EvidenceBuilder.NewValid(e => e.SendingByPost = false))
            //    .Insert(f => f.Evidence.AddFiles(f, 2));

            //existingForm.Evidence.SendingByPost.Should().BeFalse("not set before executing command");
            //existingForm.Evidence.Files.Count.Should().Be(2, "should have existing uploaded files");

            //var cmd = new AddEvidence
            //{
            //    FormId = "form123",
            //    Evidence = EvidenceBuilder.NewValid(),
            //};

            //cmd.Execute();

            //var updatedForm = Repository.Load<BestStartGrant>("form123");
            //updatedForm.Evidence.SendingByPost.Should().Be(cmd.Evidence.SendingByPost);
            //updatedForm.Evidence.Files.Count.Should().Be(2, "files should not be overwritten");
        }
    }
}
