using System;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class RelationDetailsBuilder
    {
        public static RelationDetails NewValid(Part part)
        {
            return NewValid(part, "relation");
        }

        public static RelationDetails NewValid(Part part, Action<RelationDetails> mutator = null)
        {
            return NewValid(part, "relation", mutator);
        }

        public static RelationDetails NewValid(Part part, string relation = "relation ", Action<RelationDetails> mutator = null)
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
                value.InheritAddress = false;
                value.Address = AddressBuilder.NewValid(relation);
            }

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
