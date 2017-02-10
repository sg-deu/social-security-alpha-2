using FormUI.Tests.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [SetUp]
        public void SetUp()
        {
            TestRepository.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            TestRepository.TearDown();
        }
    }
}
