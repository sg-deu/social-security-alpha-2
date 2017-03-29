using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_ExistingChildrenTests : DomainTest
    {
        [Test]
        public void AddExistingChildren_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            ExistingChildrenShouldBeValid(form, m => { });
            ExistingChildrenShouldBeValid(form, m => m.Children = new List<ExistingChild>());
            ExistingChildrenShouldBeValid(form, m => { m.Children[0].ChildBenefit = true; m.Children[0].NoChildBenefitReason = null; });

            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].FirstName = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].Surname = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].DateOfBirth = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].DateOfBirth = TestNowUtc);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].Relationship = null);
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].ChildBenefit = null);
            ExistingChildrenShouldBeInvalid(form, m => { m.Children[0].ChildBenefit = false; m.Children[0].NoChildBenefitReason = null; });
            ExistingChildrenShouldBeInvalid(form, m => m.Children[0].FormalKinshipCare = null);
        }

        protected void ExistingChildrenShouldBeValid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            ShouldBeValid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(2, mutator)));
        }

        protected void ExistingChildrenShouldBeInvalid(BestStartGrant form, Action<ExistingChildren> mutator)
        {
            ShouldBeInvalid(() => form.AddExistingChildren(ExistingChildrenBuilder.NewValid(2, mutator)));
        }
    }
}
