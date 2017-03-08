using System;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class GuardianDetailsBuilder
    {
        public static GuardianDetails NewValid(Part part, Action<GuardianDetails> mutator = null)
        {
            var value = new GuardianDetails
            {
                Title = "guardian title",
                FullName = "guardian name",
                DateOfBirth = DomainRegistry.NowUtc().Date.AddYears(-45),
                NationalInsuranceNumber = "BC123456D",
                RelationshipToApplicant = "Parent",
            };

            if (part == Part.Part2)
            {
                value.Address = AddressBuilder.NewValid();
            }

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
