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
    public class BestStartGrant_PartnerDetailsTests : DomainTest
    {
        [Test]
        public void AddPartnerDetails_Validation()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            PartnerDetailsShouldBeValid(form, Part.Part1, m => { });
            PartnerDetailsShouldBeValid(form, Part.Part2, m => { });

            PartnerDetailsShouldBeInvalid(form, Part.Part1, m => m.FullName = null);
            PartnerDetailsShouldBeInvalid(form, Part.Part2, m => m.Address.Line1 = null);
        }

        [Test]
        public void AddPartnerDetails_SetsRelationshipAutomatically()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            form.AddPartnerDetails(Part.Part1, RelationDetailsBuilder.NewValid(Part.Part1, rd => rd.RelationshipToApplicant = null));

            form.PartnerDetails.RelationshipToApplicant.Should().Be("Partner");
        }

        protected void PartnerDetailsShouldBeValid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeValid(() => form.AddPartnerDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }

        protected void PartnerDetailsShouldBeInvalid(BestStartGrant form, Part part, Action<RelationDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddPartnerDetails(part, RelationDetailsBuilder.NewValid(part, mutator)));
        }
    }
}
