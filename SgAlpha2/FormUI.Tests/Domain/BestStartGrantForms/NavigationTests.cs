using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain;
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
        [Ignore("Working on navigaing backwards")]
        public void Populate_SetsPreviousSection()
        {
            var firstSection = Navigation.Order.First();
            var lastSection = Navigation.Order.Last();

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                var detail = new BsgDetail();
                Navigation.Populate(detail, section);

                if (section == firstSection)
                    detail.PreviousSection.Should().BeNull();
                else
                    detail.PreviousSection.Should().Be(Navigation.Order.ToList()[Navigation.Order.ToList().IndexOf(section) - 1]);

                if (section == lastSection)
                    detail.IsFinalSection.Should().BeTrue();
                else
                    detail.IsFinalSection.Should().BeFalse();
            }
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
        public void RequiresGuardianDetails()
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
    }
}
