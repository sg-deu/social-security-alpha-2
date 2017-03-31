using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Commands
{
    [TestFixture]
    public class AddUKVerifyTests : DomainTest
    {
        [Test]
        public void Execute_StoresUKVerify()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .Insert();

            existingForm.UKVerify.Should().BeNull("no data stored before executing command");

            var cmd = new AddUKVerify
            {
                FormId = "form123",
                UKVerify = UKVerifyBuilder.NewValid(),
            };

            cmd.Execute();

            var updatedForm = Repository.Load<BestStartGrant>("form123");
            updatedForm.UKVerify.Should().NotBeNull();
        }
    }
}
