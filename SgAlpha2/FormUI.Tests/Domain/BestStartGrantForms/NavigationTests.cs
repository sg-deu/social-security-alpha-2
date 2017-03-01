using System;
using System.Linq;
using FluentAssertions;
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

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                var detail = new BsgDetail();
                Navigation.Populate(detail, section);

                if (section == firstSection)
                    detail.PreviousSection.Should().BeNull();
                else
                    detail.PreviousSection.Should().Be(Navigation.Order.ToList()[Navigation.Order.ToList().IndexOf(section) - 1]);
            }
        }
    }
}
