using System;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class BestStartGrant_GuardianPartnerDetailsTests : DomainTest
    {
        [Test]
        public void AddGuardianPartnerDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianPartnerDetailsShouldBeValid(form, Part.Part1, m => { });

            GuardianPartnerDetailsShouldBeInvalid(form, Part.Part1, m => m.FullName = null);
            GuardianPartnerDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line1 = null);
        }

        [Test]
        public void AddGuardianPartnerDetails_FormatsNationalInsuranceNumber()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var details = RelationDetailsBuilder.NewValid(Part.Part1, d => d.NationalInsuranceNumber = "AB123456C");
            form.AddGuardianPartnerDetails(Part.Part1, details);

            form.GuardianPartnerDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
        }

        protected void GuardianPartnerDetailsShouldBeValid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeValid(() => form.AddGuardianPartnerDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }

        protected void GuardianPartnerDetailsShouldBeInvalid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianPartnerDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }
    }
}
