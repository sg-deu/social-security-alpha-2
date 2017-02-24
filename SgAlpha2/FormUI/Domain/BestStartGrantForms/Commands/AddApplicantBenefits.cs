using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddApplicantBenefits : Command
    {
        public string               FormId;
        public Part?                Part;
        public ApplicantBenefits    ApplicantBenefits;

        public override void Execute()
        {
            if (!Part.HasValue)
                throw new Exception("Must supply AddApplicantBenefits.Part");

            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddApplicantBenefits(Part.Value, ApplicantBenefits);
        }
    }
}