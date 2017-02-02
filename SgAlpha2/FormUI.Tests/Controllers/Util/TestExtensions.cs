using FluentAssertions;
using FluentAssertions.Primitives;

namespace FormUI.Tests.Controllers.Util
{
    public static class TestExtensions
    {
        public static AndConstraint<StringAssertions> BeAction(this StringAssertions assertions, string expectedAction, string because = "", params object[] becauseArgs)
        {
            return assertions.EndWith(expectedAction.TrimStart('~'), because, becauseArgs);
        }
    }
}
