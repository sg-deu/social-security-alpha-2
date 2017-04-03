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

            PartnerDetailsShouldBeValid(form, m => { });

            PartnerDetailsShouldBeInvalid(form, m => m.FullName = null);
            PartnerDetailsShouldBeInvalid(form, m => m.Address.Line1 = null);
        }

        [Test]
        public void AddPartnerDetails_SetsRelationshipAutomatically()
        {
            var form = new BestStartGrantBuilder("form").Insert();

            form.AddPartnerDetails(RelationDetailsBuilder.NewValid(rd => rd.RelationshipToApplicant = null));

            form.PartnerDetails.RelationshipToApplicant.Should().Be("Partner");
        }

        [Test]
        public void AddPartnerDetails_AllowsAddressToBeInherited()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.PartnerDetails, RelationDetailsBuilder.NewValid())
                .Insert();

            var inheritedDetails = RelationDetailsBuilder.NewValid(rd =>
            {
                rd.InheritAddress = true;
                rd.Address = null;
            });

            form.AddPartnerDetails(inheritedDetails);

            form.PartnerDetails.InheritAddress.Should().BeTrue();
            form.PartnerDetails.Address.Should().BeNull();
        }

        protected void PartnerDetailsShouldBeValid(BestStartGrant form, Action<RelationDetails> mutator)
        {
            ShouldBeValid(() => form.AddPartnerDetails(RelationDetailsBuilder.NewValid(mutator)));
        }

        protected void PartnerDetailsShouldBeInvalid(BestStartGrant form, Action<RelationDetails> mutator)
        {
            ShouldBeInvalid(() => form.AddPartnerDetails(RelationDetailsBuilder.NewValid(mutator)));
        }
    }
}
