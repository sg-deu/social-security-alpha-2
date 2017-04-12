using FluentAssertions;
using FluentAssertions.Primitives;
using FormUI.Tests.Controllers.Util.Html;

namespace FormUI.Tests.Controllers.Util
{
    public static class TestExtensions
    {
        public static AndConstraint<StringAssertions> BeAction(this StringAssertions assertions, string expectedAction, string because = "", params object[] becauseArgs)
        {
            return assertions.EndWith(expectedAction.TrimStart('~'), because, becauseArgs);
        }

        public static void ShouldBeHidden(this ElementWrapper element)
        {
            element.Attribute("style").Should().Be("display:none");
        }
    }
}
