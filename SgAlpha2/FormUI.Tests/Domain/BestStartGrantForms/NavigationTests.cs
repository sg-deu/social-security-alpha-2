using System;
using System.Linq;
using FluentAssertions;
using FormUI.Domain;
using FormUI.Domain.BestStartGrantForms;
using FormUI.Domain.BestStartGrantForms.Responses;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms
{
    [TestFixture]
    public class NavigationTests
    {
        [Test]
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

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                var next = Navigation.Next(form, section);

                next.Id.Should().Be("form123");

                if (FeatureToggles.WorkingOnGuardianDetails && section == Sections.ApplicantDetails)
                {
                    next.Section.Should().Be(Sections.ExpectedChildren);
                    continue;
                }

                if (section == lastSection)
                    next.Section.Should().BeNull();
                else
                    next.Section.Should().Be(Navigation.Order.ToList()[Navigation.Order.ToList().IndexOf(section) + 1]);
            }
        }
    }
}
