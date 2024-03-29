﻿using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FormUI.Controllers.Helpers;
using FormUI.Tests.Controllers.Util.Html;

namespace FormUI.Tests.Controllers.Util
{
    public static class AjaxActionExtensions
    {
        public static AjaxAction ForFormGroup<T>(this AjaxAction[] actions, Expression<Func<T, object>> property)
        {
            var target = property.GetExpressionText() + "_FormGroup";
            var action = actions.Where(a => a.TargetId == target).Single();
            return action;
        }

        public static void ShouldShowHide(this AjaxAction action, DocumentWrapper doc, bool visible)
        {
            var target = doc.Find("#" + action.TargetId);
            target.Should().NotBeNull("Could not find {0} in form", action.TargetId);
            action.Action.Should().Be("ShowHide", "action should be ShowHide");
            action.Show.Should().Be(visible, "action should have Show=" + visible);
        }
    }
}
