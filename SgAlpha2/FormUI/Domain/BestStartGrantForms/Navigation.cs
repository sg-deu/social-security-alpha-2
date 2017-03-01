using System.Collections.Generic;
using FormUI.Domain.BestStartGrantForms.Responses;

namespace FormUI.Domain.BestStartGrantForms
{
    public static class Navigation
    {
        private static readonly IList<Sections> _order = new List<Sections>
        {
            Sections.Consent,
            Sections.ApplicantDetails,
            Sections.ExpectedChildren,
            Sections.ExistingChildren,
            Sections.ApplicantBenefits1,
            Sections.ApplicantBenefits2,
            Sections.HealthProfessional,
            Sections.PaymentDetails,
            Sections.Declaration,
        };

        public static IEnumerable<Sections> Order { get { return _order; } }

        public static void Populate(BsgDetail detail, Sections section)
        {
            var index = _order.IndexOf(section);

            if (index > 0)
                detail.PreviousSection = _order[index - 1];
        }
    }
}
