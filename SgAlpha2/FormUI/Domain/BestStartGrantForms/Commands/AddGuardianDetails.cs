using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddGuardianDetails : Command<NextSection>
    {
        public string           FormId;
        public RelationDetails  GuardianDetails;

        public override NextSection Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddGuardianDetails(GuardianDetails);
        }
    }
}