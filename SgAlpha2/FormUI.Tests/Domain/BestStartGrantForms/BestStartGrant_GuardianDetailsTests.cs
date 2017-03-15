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
    public class BestStartGrant_GuardianDetailsTests : DomainTest
    {
        [Test]
        public void AddGuardianDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            GuardianDetailsShouldBeValid(form, Part.Part1, m => { });
            GuardianDetailsShouldBeValid(form, Part.Part1, m => m.Title = null);
            GuardianDetailsShouldBeValid(form, Part.Part1, m => m.Address.Line3 = null);
            GuardianDetailsShouldBeValid(form, Part.Part2, m => { });

            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.FullName = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.DateOfBirth = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.DateOfBirth = TestNowUtc);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.NationalInsuranceNumber = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part1, m => m.RelationshipToApplicant = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line1 = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line2 = null);
            GuardianDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Postcode = null);
        }

        [Test]
        public void AddGuardianDetails_FormatsNationalInsuranceNumber()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var details = RelationDetailsBuilder.NewValid(Part.Part1, d => d.NationalInsuranceNumber = "AB123456C");
            form.AddGuardianDetails(Part.Part1, details);

            form.GuardianDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
        }

        protected void GuardianDetailsShouldBeValid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeValid(() => form.AddGuardianDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }

        protected void GuardianDetailsShouldBeInvalid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }
    }
}
