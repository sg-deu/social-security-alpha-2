using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class ExistingChildrenBuilder
    {
        public static ExistingChildren NewValid(Action<ExistingChildren> mutator = null)
        {
            var value = new ExistingChildren
            {
                Children = new List<ExistingChild>()
                {
                    new ExistingChild()
                    {
                        FirstName = "child 1 first name",
                        Surname = "child 1 surname",
                        DateOfBirth = DomainRegistry.NowUtc().Date - TimeSpan.FromDays(10 * 365),
                        RelationshipToChild = "parent",
                        ChildBenefit = true,
                        FormalKinshipCare = true,
                    },
                    new ExistingChild()
                    {
                        FirstName = "child 2 first name",
                        Surname = "child 2 surname",
                        DateOfBirth = DomainRegistry.NowUtc().Date - TimeSpan.FromDays(12 * 365),
                        RelationshipToChild = "guardian",
                        ChildBenefit = false,
                        FormalKinshipCare = false,
                    },
                },
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
