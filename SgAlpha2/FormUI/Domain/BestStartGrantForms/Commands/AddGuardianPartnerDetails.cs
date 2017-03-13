using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddGuardianPartnerDetails : Command<NextSection>
    {
        public string           FormId;
        public Part?            Part;
        public RelationDetails  GuardianPartnerDetails;

        public override NextSection Execute()
        {
            if (!Part.HasValue)
                throw new Exception("Must supply AddGuardianPartnerDetails.Part");

            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddGuardianPartnerDetails(Part.Value, GuardianPartnerDetails);
        }
    }
}