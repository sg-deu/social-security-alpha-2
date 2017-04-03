using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddPartnerDetails : Command<NextSection>
    {
        public string           FormId;
        public RelationDetails  PartnerDetails;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddPartnerDetails(PartnerDetails);
        }
    }
}