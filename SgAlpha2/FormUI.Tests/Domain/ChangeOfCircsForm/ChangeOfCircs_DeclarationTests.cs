using System;
using FluentAssertions;
using FormUI.Domain.ChangeOfCircsForm;
using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.ChangeOfCircsForm.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.ChangeOfCircsForm
{
    [TestFixture]
    public class ChangeOfCircs_DeclarationTests : DomainTest
    {
        [Test]
        public void Complete_SetsCompletionDate()
        {
            var form = new ChangeOfCircsBuilder("form")
                .WithCompletedSections(markAsCompleted: false)
                .With(f => f.Declaration, null)
                .Insert();

            var next = form.AddDeclaration(DeclarationBuilder.NewValid());

            next.Section.Should().BeNull("this should be the last section that is filled out");
            form.Completed.Should().Be(TestNowUtc.Value);
        }

        [Test]
        public void Declaration_Validation()
        {
            var form = new ChangeOfCircsBuilder("form").Insert();

            DeclarationShouldBeValid(form, m => { });

            DeclarationShouldBeInvalid(form, m => m.AgreedToLegalStatement = false);
        }

        protected void DeclarationShouldBeValid(ChangeOfCircs form, Action<Declaration> mutator)
        {
            ShouldBeValid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }

        protected void DeclarationShouldBeInvalid(ChangeOfCircs form, Action<Declaration> mutator)
        {
            ShouldBeInvalid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }
    }
}
