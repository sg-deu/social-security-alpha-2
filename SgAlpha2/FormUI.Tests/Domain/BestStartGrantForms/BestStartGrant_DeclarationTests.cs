using System;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_DeclarationTests : DomainTest
    {
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
