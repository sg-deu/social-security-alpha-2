using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Commands;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Commands
{
    [TestFixture]
    public class AddEvidenceTests : DomainTest
    {
        [Test]
        public void Execute_StoresEvidence()
        {
            var existingForm = new ChangeOfCircsBuilder("form123")
                .With(f => f.Evidence, EvidenceBuilder.NewValid(e => e.SendingByPost = false))
                .Insert(f => f.Evidence.AddFiles(f, 2));

            existingForm.Evidence.SendingByPost.Should().BeFalse("not set before executing command");
            existingForm.Evidence.Files.Count.Should().Be(2, "should have existing uploaded files");

            var cmd = new AddEvidence
            {
                FormId = "form123",
                Evidence = EvidenceBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<ChangeOfCircs>("form123");
            updatedForm.Evidence.SendingByPost.Should().Be(cmd.Evidence.SendingByPost);
            updatedForm.Evidence.Files.Count.Should().Be(2, "files should not be overwritten");
        }
    }
}
