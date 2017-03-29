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
                children.Add(NewChild(i));

            var value = new ExistingChildren
            {
                AnyExistingChildren = (children.Count > 0),
                Children = children,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }

        private static ExistingChild NewChild(int index)
        {
            return new ExistingChild
            {
                FirstName = $"child {index + 1} first name",
                Surname = $"child {index + 1} surname",
                DateOfBirth = DomainRegistry.NowUtc().Date.AddYears(-(10 + index * 2)),
                Relationship = Relationship.Parent,
                ChildBenefit = false,
                NoChildBenefitReason = $"unit test reason {index + 1}",
            };
        }

        public static ExistingChildren LastNotKinshipCare(this ExistingChildren existingChildren)
        {
            for (var i = 0; i < existingChildren.Children.Count; i++)
                existingChildren.Children[i].Relationship = (i != existingChildren.Children.Count - 1) ? Relationship.KinshipCarer : Relationship.Parent;

            return existingChildren;
        }

        public static ExistingChildren AllKinshipCare(this ExistingChildren existingChildren)
        {
            for (var i = 0; i < existingChildren.Children.Count; i++)
                existingChildren.Children[i].Relationship = Relationship.KinshipCarer;

            return existingChildren;
        }

        public static ExistingChildren AddChild(this ExistingChildren existingChildren)
        {
            existingChildren.Children.Add(NewChild(existingChildren.Children.Count));
            return existingChildren;
        }
    }
}
