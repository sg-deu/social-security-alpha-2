using System;
using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms
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
                        DateOfBirth = new DateTime(2008, 07, 06),
                        RelationshipToChild = "parent",
                        ChildBenefit = true,
                        FormalKinshipCare = true,
                    },
                    new ExistingChild()
                    {
                        FirstName = "child 2 first name",
                        Surname = "child 2 surname",
                        DateOfBirth = new DateTime(2009, 08, 07),
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
