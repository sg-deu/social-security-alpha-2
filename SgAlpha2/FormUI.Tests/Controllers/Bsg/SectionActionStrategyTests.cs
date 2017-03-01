using System;
using System.Collections.Generic;
using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Domain.BestStartGrantForms;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class SectionActionStrategyTests
    {
        [Test]
        public void AllSectionsHaveActions()
        {
            var sectionActions = new List<string>();

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                var strategy = SectionActionStrategy.For(section);
                var action = strategy.Action("formId");
                action.Should().NotBeNullOrWhiteSpace();
                action.Should().Contain("formId");

                sectionActions.Contains(action).Should().BeFalse("Action {0} should only be used once", action);
                sectionActions.Add(action);
            }
        }
    }
}
