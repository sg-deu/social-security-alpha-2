using System;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;
using FormUI.Tests.Domain.Forms.Dto;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    public class RelationDetailsBuilder
    {
        public static RelationDetails NewValid()
        {
            return NewValid("relation");
        }

        public static RelationDetails NewValid(Action<RelationDetails> mutator = null)
        {
            return NewValid("relation", mutator);
        }

        public static RelationDetails NewValid(string relation = "relation ", Action<RelationDetails> mutator = null)
        {
            var value = new RelationDetails
            {
                Title = "relation title",
                FullName = "relation name",
                DateOfBirth = DomainRegistry.NowUtc().Date.AddYears(-45),
                NationalInsuranceNumber = "BC123456D",
                RelationshipToApplicant = "Parent",

                InheritAddress = false,
                Address = AddressBuilder.NewValid(relation),
            };

            if (mutator != null)
                mutator(value);

            return value;
        }
    }
}
