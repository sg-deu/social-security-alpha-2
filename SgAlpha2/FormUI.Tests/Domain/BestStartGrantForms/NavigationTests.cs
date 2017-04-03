using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Responses;
using FormUI.Tests.Domain.BestStartGrantForms.Dto;
using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class NavigationTests : DomainTest
    {
        [Test]
        public void Populate_SetsPreviousSection()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Under16(TestNowUtc.Value))
                .Value();

            var detail = new BsgDetail();

            Navigation.Populate(detail, Sections.GuardianDetails, form);

            detail.PreviousSection.Should().Be(Sections.ExistingChildren,
                "Navigating backwards from guardian details for a 16 year old should go back to existing children (and skip applicant benefits)");
        }

        [Test]
        public void Populate_NoPreviousSectionFromFirstSection()
        {
            var form = new BestStartGrantBuilder("form")
                .Value();

            var detail = new BsgDetail();

            Navigation.Populate(detail, Navigation.Order.First(), form);

            detail.PreviousSection.Should().BeNull();
        }

        [Test]
        public void Next_ReturnsNextSection()
        {
            var lastSection = Navigation.Order.Last();
            var form = new BestStartGrantBuilder("form123").WithCompletedSections().Value();
            bool lastSectionReached = false;

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                var next = Navigation.Next(form, section);
                next.Id.Should().Be("form123");

                if (section == lastSection)
                    next.Section.Should().BeNull();
                else if (next.Section == lastSection)
                    lastSectionReached = true;
            }

            lastSectionReached.Should().BeTrue("last section should be reached");
        }

        [Test]
        public void RequiresApplicantBenefits()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Value();

            TestNowUtc = new DateTime(2009, 08, 07, 06, 05, 04);

            form.ApplicantDetails.DateOfBirth = null;

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue();

            // 18 tomorrow
            form.ApplicantDetails.DateOfBirth = new DateTime(1991, 08, 08);

            Navigation.RequiresApplicantBenefits(form).Should().BeFalse("under 18 does not require applicant benefits");

            // 18 today
            form.ApplicantDetails.DateOfBirth = new DateTime(1991, 08, 07);

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue();
        }

        [Test]
        public void RequiresApplicantBenefits_NotRequiredWhenAskingGuardianBenefits()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Value();

            form.ApplicantDetails.Aged(TestNowUtc.Value, 18);
            form.ApplicantDetails.FullTimeEducation = false;
            BestStartGrant.ShouldAskEducationQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should ask for applicant benefits if not asking for guardian benefits");

            form.ApplicantDetails.Aged(TestNowUtc.Value, 19);
            form.ApplicantDetails.FullTimeEducation = true;
            BestStartGrant.ShouldAskEducationQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresApplicantBenefits(form).Should().BeFalse("should not ask for applicant benefits if asking for guardian benefits");
        }

        [Test]
        public void RequiresApplicantBenefits_NotRequiredWhenAllChildrenKinshipCare()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid(0))
                .Value();

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should ask for applicant benefits if no existing children with kinship care");

            Builder.Modify(form).With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid(3).LastNotKinshipCare());

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should ask for applicant benefits if not all children in kinship care");

            Builder.Modify(form).With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid(3).AllKinshipCare());

            Navigation.RequiresApplicantBenefits(form).Should().BeFalse("should not ask for applicant benefits is all children are kinship care");

            Builder.Modify(form).With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid(m => { m.ExpectancyDate = null; m.ExpectedBabyCount = 1; }));

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should not ask for applicant benefits, since an expected child is not expected to be in kinship care");

            Builder.Modify(form).With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid(m => { m.ExpectancyDate = TestNowUtc.Value; m.ExpectedBabyCount = null; }));

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should not ask for applicant benefits, since an expected child is not expected to be in kinship care");
        }

        [Test]
        public void RequiresApplicantBenefits_NotRequiredWhenCareLeaver()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Value();

            form.ApplicantDetails.DateOfBirth = null;

            Navigation.RequiresGuardianBenefits(form).Should().BeFalse();

            form.ApplicantDetails.Over25(TestNowUtc.Value);
            form.ApplicantDetails.PreviouslyLookedAfter = true;
            BestStartGrant.ShouldAskCareQuestion(form.ApplicantDetails).Should().BeFalse("question not relevant");

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should still ask for benefits if not under 25 (despite previous care)");

            form.ApplicantDetails.Aged(TestNowUtc.Value, 23);
            form.ApplicantDetails.PreviouslyLookedAfter = false;
            BestStartGrant.ShouldAskCareQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresApplicantBenefits(form).Should().BeTrue("should still ask for benefits if not previously in care");

            form.ApplicantDetails.Aged(TestNowUtc.Value, 23);
            form.ApplicantDetails.PreviouslyLookedAfter = true;
            BestStartGrant.ShouldAskCareQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresApplicantBenefits(form).Should().BeFalse("should ask for guardian benefits if responded in full time education");
        }

        [Test]
        public void RequiresPartnerBenefits()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantBenefits, null)
                .Value();

            Navigation.RequiresPartnerBenefits(form).Should().BeFalse("partner benefits not required if applicant benefits not asked");

            Builder.Modify(form).With(f => f.ApplicantBenefits, BenefitsBuilder.NewWithBenefit());

            Navigation.RequiresPartnerBenefits(form).Should().BeFalse("partner benefits not required if applicant benefits positive");

            form.ApplicantBenefits.None();

            Navigation.RequiresPartnerBenefits(form).Should().BeTrue("partner benefits required if applicant benefits asked but answered 'no'");

            form.ApplicantBenefits.Unknown();

            Navigation.RequiresPartnerBenefits(form).Should().BeTrue("partner benefits required if applicant benefits asked but answered 'don't know'");
        }

        [Test]
        public void RequiresPartnerBenefits_NotRequiredWhenAnyBenefitsNotRequired()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Under25CareLeaver(TestNowUtc.Value))
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewEmpty(b => b.None = true))
                .Value();

            Navigation.RequiresPartnerBenefits(form).Should().BeFalse("should not prompt for benefits when already entitled");
        }

        [Test]
        public void RequiresGuardianBenefits()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Value();

            form.ApplicantDetails.DateOfBirth = null;

            Navigation.RequiresGuardianBenefits(form).Should().BeFalse();

            form.ApplicantDetails.Over25(TestNowUtc.Value);
            form.ApplicantDetails.FullTimeEducation = true;
            BestStartGrant.ShouldAskEducationQuestion(form.ApplicantDetails).Should().BeFalse("question not relevant");

            Navigation.RequiresGuardianBenefits(form).Should().BeFalse("should not ask for guardian benefits if not 18 or 19");

            form.ApplicantDetails.Aged(TestNowUtc.Value, 18);
            form.ApplicantDetails.FullTimeEducation = false;
            BestStartGrant.ShouldAskEducationQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresGuardianBenefits(form).Should().BeFalse("should not ask for guardian benefits if not in full time education");

            form.ApplicantDetails.Aged(TestNowUtc.Value, 19);
            form.ApplicantDetails.FullTimeEducation = true;
            BestStartGrant.ShouldAskEducationQuestion(form.ApplicantDetails).Should().BeTrue("ensure question is asked");

            Navigation.RequiresGuardianBenefits(form).Should().BeTrue("should ask for guardian benefits if responded in full time education");
        }

        [Test]
        public void RequiresGuardianPartnerBenefits()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.GuardianBenefits, null)
                .Value();

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeFalse("guardian partner benefits not required if guardian benefits not asked");

            Builder.Modify(form).With(f => f.GuardianBenefits, BenefitsBuilder.NewWithBenefit());

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeFalse("guardian partner benefits not required if guardian benefits positive");

            form.GuardianBenefits.None();

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeTrue("guardian partner benefits required if guardian benefits asked but answered 'no'");

            form.GuardianBenefits.Unknown();

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeTrue("guardian partner benefits required if guardian benefits asked but answered 'don't know'");
        }

        [Test]
        public void RequiresGuardianPartnerBenefits_NotRequiredWhenAnyBenefitsNotRequired()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Under25CareLeaver(TestNowUtc.Value))
                .With(f => f.GuardianBenefits, BenefitsBuilder.NewNone())
                .Value();

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeFalse("should not prompt for benefits when already entitled");
        }

        [Test]
        public void RequiresPartnerDetails()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Over25(TestNowUtc.Value))
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewNone())
                .With(f => f.PartnerBenefits, null)
                .Value();

            Navigation.RequiresPartnerBenefits(form).Should().BeTrue("test logic requires that the partner benefits are requested");

            Navigation.RequiresPartnerDetails(form).Should().BeFalse("until partner benefits are collected, we don't need their details");

            Builder.Modify(form).With(f => f.PartnerBenefits, BenefitsBuilder.NewNone());

            Navigation.RequiresPartnerDetails(form).Should().BeFalse("partner details not required if we know they don't have a qualifying benefit");

            form.PartnerBenefits.WithBenefit();

            Navigation.RequiresPartnerDetails(form).Should().BeTrue("partner details required if relying in their qualifying benefits");

            form.PartnerBenefits.Unknown();

            Navigation.RequiresPartnerDetails(form).Should().BeTrue("partner details required if not sure if relying on their benefits");
        }

        [Test]
        public void RequiresGuardianDetails_FullTimeEducation()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().PartOfGuardianBenefits(TestNowUtc.Value))
                .With(f => f.GuardianBenefits, null)
                .Value();

            Navigation.RequiresGuardianDetails(form).Should().BeFalse("until guardian benefits are collected, we don't need their details");

            Builder.Modify(form).With(f => f.GuardianBenefits, BenefitsBuilder.NewNone());

            Navigation.RequiresGuardianDetails(form).Should().BeFalse("guardian details not required if we know they don't have a qualifying benefit");

            form.GuardianBenefits.WithBenefit();

            Navigation.RequiresGuardianDetails(form).Should().BeTrue("guardian details required if relying on their qualifying benefits");

            form.GuardianBenefits.Unknown();

            Navigation.RequiresGuardianDetails(form).Should().BeTrue("guardian details required if not sure if relying on their benefits");
        }

        [Test]
        public void RequiresGuardianDetails_Under16()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid())
                .Value();

            TestNowUtc = new DateTime(2009, 08, 07, 06, 05, 04);

            form.ApplicantDetails.DateOfBirth = null;

            Navigation.RequiresGuardianDetails(form).Should().BeTrue();

            // 16 tomorrow
            form.ApplicantDetails.DateOfBirth = new DateTime(1993, 08, 08);

            Navigation.RequiresGuardianDetails(form).Should().BeTrue();

            // 16 today
            form.ApplicantDetails.DateOfBirth = new DateTime(1993, 08, 07);

            Navigation.RequiresGuardianDetails(form).Should().BeFalse("over 16 does not require a legal guradian/parent");
        }

        [Test]
        public void RequiresGuardianPartnerDetails()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().PartOfGuardianBenefits(TestNowUtc.Value))
                .Value();

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeFalse("until guardian partner benefits are collected, we don't need their details");

            Builder.Modify(form)
                .With(f => f.GuardianBenefits, BenefitsBuilder.NewNone())
                .With(f => f.GuardianPartnerBenefits, BenefitsBuilder.NewNone());

            Navigation.RequiresGuardianBenefits(form).Should().BeTrue("test logic requires that the guardian benefits are requested");

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeFalse("guardian partner details not required if we know they don't have a qualifying benefit");

            form.GuardianPartnerBenefits.WithBenefit();

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeTrue("guardian partner details required if relying on their benefits");

            form.GuardianPartnerBenefits.Unknown();

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeTrue("guardian partner details required if not sure if relying on their benefits");
        }

        [Test]
        public void Ineligible_NotIneligible()
        {
            var form = new BestStartGrantBuilder("form").WithCompletedSections().Value();

            Navigation.IsIneligible(form, Navigation.Order.Last()).Should().BeFalse();
        }

        [Test]
        public void Ineligible_NoChildren()
        {
            Func<Action<BestStartGrant>, BestStartGrant> form = mutator => new BestStartGrantBuilder("form")
                .WithCompletedSections()
                .With(f => f.ExistingChildren, ExistingChildrenBuilder.NewValid(childCount: 0))
                .With(f => f.ExpectedChildren, ExpectedChildrenBuilder.NewValid(ec => ec.NoBabyExpected()))
                .Value(mutator);

            var lastChildSection = (Sections)Math.Max((int)Sections.ExistingChildren, (int)Sections.ExpectedChildren);

            Navigation.IsIneligible(form(f => { }), lastChildSection).Should().BeTrue("not eligible when there are no existing or expected children");

            Navigation.IsIneligible(form(f => { }), lastChildSection - 1).Should().BeFalse("ineligibility is not determined until both children sections are complete");
            Navigation.IsIneligible(form(f => { }), lastChildSection - 1).Should().BeFalse("ineligibility is not determined until both children sections are complete");
            Navigation.IsIneligible(form(f => f.ExpectedChildren.ExpectedBabyCount(1)), lastChildSection).Should().BeFalse("not ineligible if you are due a baby");
            Navigation.IsIneligible(form(f => f.ExpectedChildren.ExpectedBabyCount(2)), lastChildSection).Should().BeFalse("not ineligible if you are due a baby");
            Navigation.IsIneligible(form(f => f.ExistingChildren.AddChild()), lastChildSection).Should().BeFalse("potentially not ineligible if you have an existing child");
        }

        [Test]
        public void Ineligible_NoPartnerBenefits()
        {
            Func<Action<BestStartGrant>, BestStartGrant> form = mutator => new BestStartGrantBuilder("form")
                .WithCompletedSections()
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewNone())
                .With(f => f.PartnerBenefits, BenefitsBuilder.NewNone())
                .Value(mutator);

            var lastBenefitsSection = Sections.PartnerBenefits;

            Navigation.IsIneligible(form(f => { }), lastBenefitsSection).Should().BeTrue("not eligible when applicant or partner has no benefits");

            Navigation.IsIneligible(form(f => { }), lastBenefitsSection - 1).Should().BeFalse("ineligibility is not determined until both applicant and partner benefits are complete");
            Navigation.IsIneligible(form(f => f.PartnerBenefits.WithBenefit()), lastBenefitsSection).Should().BeFalse("not ineligible if partner is on a benefit");
            Navigation.IsIneligible(form(f => f.ApplicantBenefits.Unknown()), lastBenefitsSection).Should().BeFalse("cannot assume inelgible if applicant benefit not known");
            Navigation.IsIneligible(form(f => f.PartnerBenefits.Unknown()), lastBenefitsSection).Should().BeFalse("cannot assume ineligible if partner benefit not known");
        }

        [Test]
        public void Ineligible_NoGuardianPartnerBenefits()
        {
            Func<Action<BestStartGrant>, BestStartGrant> form = mutator => new BestStartGrantBuilder("form")
                .WithCompletedSections()
                .With(f => f.GuardianBenefits, BenefitsBuilder.NewNone())
                .With(f => f.GuardianPartnerBenefits, BenefitsBuilder.NewNone())
                .Value(mutator);

            var lastBenefitsSection = Sections.GuardianPartnerBenefits;

            Navigation.IsIneligible(form(f => { }), lastBenefitsSection).Should().BeTrue("not eligible when guardian's partner has no benefits");

            Navigation.IsIneligible(form(f => { }), lastBenefitsSection - 1).Should().BeFalse("ineligibility not determined until both guardian and guardian partner benefits are complete");
            Navigation.IsIneligible(form(f => f.GuardianPartnerBenefits.WithBenefit()), lastBenefitsSection).Should().BeFalse("not ineligible if guardian's partner on a benefit");
            Navigation.IsIneligible(form(f => f.GuardianBenefits.Unknown()), lastBenefitsSection).Should().BeFalse("cannot assume ineligible if guardian benefit not known");
            Navigation.IsIneligible(form(f => f.GuardianPartnerBenefits.Unknown()), lastBenefitsSection).Should().BeFalse("cannot assume ineligible if guardian partner benefit not known");
        }
    }
}
