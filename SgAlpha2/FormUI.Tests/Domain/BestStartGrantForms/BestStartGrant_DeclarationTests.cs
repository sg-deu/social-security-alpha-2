using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_DeclarationTests : DomainTest
    {
        [Test]
        public void Complete_SetsCompletionDate()
        {
            var form = new BestStartGrantBuilder("form")
                .WithCompletedSections(markAsCompleted: false)
                .With(f => f.Declaration, null)
                .Insert();

            var next = form.AddDeclaration(DeclarationBuilder.NewValid());

            next.Section.Should().BeNull("this should be the last section that is filled out");
            form.Completed.Should().Be(TestNowUtc.Value);
        }

        [Test]
        public void Complete_ThrowsIfAlreadyCompleted()
        {
            var form = new BestStartGrantBuilder("form")
                .WithCompletedSections()
                .Value();

            form.Completed.Should().HaveValue();

            Assert.Throws<DomainException>(() =>
                form.AddConsent(ConsentBuilder.NewValid()));
        }

        [Test]
        public void Complete_DeclarationValidated()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            DeclarationShouldBeValid(form, m => { });

            DeclarationShouldBeInvalid(form, m => m.AgreedToLegalStatement = false);
        }

        protected void DeclarationShouldBeValid(BestStartGrant form, Action<Declaration> mutator)
        {
            ShouldBeValid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }

        protected void DeclarationShouldBeInvalid(BestStartGrant form, Action<Declaration> mutator)
        {
            ShouldBeInvalid(() => form.AddDeclaration(DeclarationBuilder.NewValid(mutator)));
        }
    }
}
