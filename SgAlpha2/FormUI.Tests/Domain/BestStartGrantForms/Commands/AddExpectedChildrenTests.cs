using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddExpectedChildrenTests : DomainTest
    {
        [Test]
        public void Execute_StoresExpectedDetails()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Insert();

            existingForm.ExpectedChildren.Should().BeNull("no data stored before executing command");

            var cmd = new AddExpectedChildren
            {
                FormId = "form123",
                ExpectedChildren = ExpectedChildrenBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.ExpectedChildren.Should().NotBeNull();
            updatedForm.ExpectedChildren.ExpectancyDate.Should().Be(cmd.ExpectedChildren.ExpectancyDate);
        }
    }
}
