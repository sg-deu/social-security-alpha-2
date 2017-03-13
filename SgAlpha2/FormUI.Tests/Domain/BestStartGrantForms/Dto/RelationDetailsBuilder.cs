using System;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class RelationDetailsBuilder
    {
        public static RelationDetails NewValid(Part part, Action<RelationDetails> mutator = null)
        {
            var value = new RelationDetails
            {
                Title = "relation title",
                FullName = "relation name",
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
