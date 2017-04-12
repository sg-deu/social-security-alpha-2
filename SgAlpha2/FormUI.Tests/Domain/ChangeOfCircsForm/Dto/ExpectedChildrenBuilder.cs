using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
{
    public static class ExpectedChildrenBuilder
    {
        public static ExpectedChildren NewValid(Action<ExpectedChildren> mutator = null)
        {
            var expectedChildren = new ExpectedChildren();
            BestStartGrantForms.Dto.ExpectedChildrenBuilder.Populate(expectedChildren);
            mutator?.Invoke(expectedChildren);
            return expectedChildren;
        }
    }
}
