using System;
using System.Collections.Generic;
using FluentAssertions;
using FormUI.Controllers.Coc;
using FormUI.Domain;
using FormUI.Domain.ChangeOfCircsForm;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Coc
{
    [TestFixture]
    public class SectionActionStrategyTests : AbstractTest
    {
        [Test]
        public void AllSectionsHaveActions()
        {
            var sectionActions = new List<string>();

            foreach (Sections section in Enum.GetValues(typeof(Sections)))
            {
                if (FeatureToggles.SkipWorkInProgressSection(section))
                    continue;

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
