using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Dto;
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

            Navigation.Populate(detail, Sections.GuardianDetails1, form);

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
            var form = new BestStartGrantBuilder("form123").Value();
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

            Builder.Modify(form).With(f => f.ApplicantBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.Yes));

            Navigation.RequiresPartnerBenefits(form).Should().BeFalse("partner benefits not required if applicant benefits positive");

            form.ApplicantBenefits.HasExistingBenefit = YesNoDk.No;

            Navigation.RequiresPartnerBenefits(form).Should().BeTrue("partner benefits required if applicant benefits asked but answered 'no'");

            form.ApplicantBenefits.HasExistingBenefit = YesNoDk.DontKnow;

            Navigation.RequiresPartnerBenefits(form).Should().BeTrue("partner benefits required if applicant benefits asked but answered 'don't know'");
        }

        [Test]
        public void RequiresPartnerBenefits_NotRequiredWhenAnyBenefitsNotRequired()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Under25CareLeaver(TestNowUtc.Value))
                .With(f => f.ApplicantBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.No))
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

            Builder.Modify(form).With(f => f.GuardianBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.Yes));

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeFalse("guardian partner benefits not required if guardian benefits positive");

            form.GuardianBenefits.HasExistingBenefit = YesNoDk.No;

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeTrue("guardian partner benefits required if guardian benefits asked but answered 'no'");

            form.GuardianBenefits.HasExistingBenefit = YesNoDk.DontKnow;

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeTrue("guardian partner benefits required if guardian benefits asked but answered 'don't know'");
        }

        [Test]
        public void RequiresGuardianPartnerBenefits_NotRequiredWhenAnyBenefitsNotRequired()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().Under25CareLeaver(TestNowUtc.Value))
                .With(f => f.GuardianBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.No))
                .Value();

            Navigation.RequiresGuardianPartnerBenefits(form).Should().BeFalse("should not prompt for benefits when already entitled");
        }

        [Test]
        public void RequiresGuardianDetails_FullTimeEducation()
        {
            var form = new BestStartGrantBuilder("form")
                .With(f => f.ApplicantDetails, ApplicantDetailsBuilder.NewValid().PartOfGuardianBenefits(TestNowUtc.Value))
                .Value();

            Navigation.RequiresGuardianDetails(form).Should().BeFalse("until guardian benefits are collected, we don't need their details");

            Builder.Modify(form).With(f => f.GuardianBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.No));

            Navigation.RequiresGuardianDetails(form).Should().BeFalse("guardian details not required if we know they don't have a qualifying benefit");

            form.GuardianBenefits.HasExistingBenefit = YesNoDk.Yes;

            Navigation.RequiresGuardianDetails(form).Should().BeTrue("guardian details required if relying on their benefits");

            form.GuardianBenefits.HasExistingBenefit = YesNoDk.DontKnow;

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
                .With(f => f.GuardianBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.No))
                .With(f => f.GuardianPartnerBenefits, BenefitsBuilder.NewValid(b => b.HasExistingBenefit = YesNoDk.No));

            Navigation.RequiresGuardianBenefits(form).Should().BeTrue("test logic requires that the guardian benefits are requested");

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeFalse("guardian partner details not required if we know they don't have a qualifying benefit");

            form.GuardianPartnerBenefits.HasExistingBenefit = YesNoDk.Yes;

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeTrue("guardian partner details required if relying on their benefits");

            form.GuardianPartnerBenefits.HasExistingBenefit = YesNoDk.DontKnow;

            Navigation.RequiresGuardianPartnerDetails(form).Should().BeTrue("guardian partner details required if not sure if relying on their benefits");
        }
    }
}
