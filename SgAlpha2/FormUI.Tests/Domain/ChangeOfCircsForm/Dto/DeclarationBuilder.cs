using System;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Tests.Domain.ChangeOfCircsForm.Dto
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
