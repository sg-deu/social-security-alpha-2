using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public static class ExistingChildrenBuilder
    {
        public static ExistingChildren NewValid(int childCount = 2, Action<ExistingChildren> mutator = null)
        {
            var children = new List<ExistingChild>();

            for (var i = 0; i < childCount; i++)
            {
                children.Add(new ExistingChild
                {
                    FirstName = $"child {i + 1} first name",
                    Surname = $"child {i + 1} surname",
                    DateOfBirth = DomainRegistry.NowUtc().Date.AddYears(-(10 + i * 2)),
                    RelationshipToChild = "guardian",
                    ChildBenefit = false,
                    NoChildBenefitReason = $"unit test reason {i + 1}",
                    FormalKinshipCare = false,
                });
            }

            var value = new ExistingChildren
            {
                Children = children,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        public static ExistingChildren LastNotKinshipCare(this ExistingChildren existingChildren)
        {
            for (var i = 0; i < existingChildren.Children.Count; i++)
                existingChildren.Children[i].FormalKinshipCare = i != existingChildren.Children.Count - 1;

            return existingChildren;
        }

        public static ExistingChildren AllKinshipCare(this ExistingChildren existingChildren)
        {
            for (var i = 0; i < existingChildren.Children.Count; i++)
                existingChildren.Children[i].FormalKinshipCare = true;

            return existingChildren;
        }
    }
}
