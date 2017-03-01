using System;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class DeclarationBuilder
    {
        public static Declaration NewValid(Action<Declaration> mutator = null)
        {
            var value = new Declaration
            {
                AgreedToLegalStatement = true,
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
