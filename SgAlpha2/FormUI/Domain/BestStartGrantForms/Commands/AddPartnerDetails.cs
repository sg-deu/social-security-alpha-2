using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddPartnerDetails : Command<NextSection>
    {
        public string           FormId;
        public Part?            Part;
        public RelationDetails  PartnerDetails;

        public override NextSection Execute()
        {
            if (!Part.HasValue)
                throw new Exception("Must supply AddPartnerDetails.Part");

            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddPartnerDetails(Part.Value, PartnerDetails);
        }
    }
}