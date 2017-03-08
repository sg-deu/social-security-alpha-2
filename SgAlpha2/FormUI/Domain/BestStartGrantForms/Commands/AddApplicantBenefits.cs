using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddApplicantBenefits : Command<NextSection>
    {
        public string               FormId;
        public Part?                Part;
        public ApplicantBenefits    ApplicantBenefits;

        public override NextSection Execute()
        {
            if (!Part.HasValue)
                throw new Exception("Must supply AddApplicantBenefits.Part");

            var form = Repository.Load<BestStartGrant>(FormId);
            return form.AddApplicantBenefits(Part.Value, ApplicantBenefits);
        }
    }
}