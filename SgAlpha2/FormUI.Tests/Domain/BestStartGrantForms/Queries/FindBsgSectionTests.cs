using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Queries;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Queries
{
    [TestFixture]
    public class FindBsgSectionTests : DomainTest
    {
        [Test]
        public void Find_PopulatesDetail()
        {
            var existingForm = new BestStartGrantBuilder("form123")
                .WithCompletedSections()
                .Insert();

            var query = new FindBsgSection
            {
                FormId = "form123",
                Section = Sections.ExistingChildren,
            };

            var detail = query.Find();

            detail.ShouldBeEquivalentTo(new BsgDetail
            {
                Consent = existingForm.Consent,
                ApplicantDetails = existingForm.ApplicantDetails,
                ExpectedChildren = existingForm.ExpectedChildren,
                ExistingChildren = existingForm.ExistingChildren,
                GuardianDetails = existingForm.GuardianDetails,
                ApplicantBenefits = existingForm.ApplicantBenefits,
                HealthProfessional = existingForm.HealthProfessional,
                PaymentDetails = existingForm.PaymentDetails,
                Declaration = existingForm.Declaration,

                PreviousSection = Sections.ExpectedChildren,
            });
        }
    }
}
