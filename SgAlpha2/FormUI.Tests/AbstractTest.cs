using NUnit.Framework;

namespace FormUI.Tests
{
    public abstract class AbstractTest
    {
        [TearDown]
        public virtual void TearDown()
        {
            var context = TestContext.CurrentContext;

            if (context.Result.Status == TestStatus.Failed)
                TestRegistry.TestHasFailed = true;
        }
    }
}
