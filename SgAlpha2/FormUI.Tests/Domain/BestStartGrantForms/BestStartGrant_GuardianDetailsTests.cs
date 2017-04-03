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

            GuardianDetailsShouldBeValid(form, m => { });
            GuardianDetailsShouldBeValid(form, m => m.Title = null);
            GuardianDetailsShouldBeValid(form, m => m.Address.Line2 = null);
            GuardianDetailsShouldBeValid(form, m => m.Address.Line3 = null);

            GuardianDetailsShouldBeInvalid(form, m => m.FullName = null);
            GuardianDetailsShouldBeInvalid(form, m => m.DateOfBirth = null);
            GuardianDetailsShouldBeInvalid(form, m => m.DateOfBirth = TestNowUtc);
            GuardianDetailsShouldBeInvalid(form, m => m.NationalInsuranceNumber = null);
            GuardianDetailsShouldBeInvalid(form, m => m.RelationshipToApplicant = null);
            GuardianDetailsShouldBeInvalid(form, m => m.Address.Line1 = null);
            GuardianDetailsShouldBeInvalid(form, m => m.Address.Postcode = null);
        }

        [Test]
        public void AddGuardianDetails_FormatsNationalInsuranceNumber()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            var details = RelationDetailsBuilder.NewValid(d => d.NationalInsuranceNumber = "AB123456C");
            form.AddGuardianDetails(details);

            form.GuardianDetails.NationalInsuranceNumber.Should().Be("AB 12 34 56 C");
        }

        protected void GuardianDetailsShouldBeValid(BestStartGrant form, Action<RelationDetails> mutator)
        {
            ShouldBeValid(() => form.AddGuardianDetails(RelationDetailsBuilder.NewValid(mutator)));
        }

        protected void GuardianDetailsShouldBeInvalid(BestStartGrant form, Action<RelationDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddGuardianDetails(RelationDetailsBuilder.NewValid(mutator)));
        }
    }
}
