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
            LocalRepository.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            LocalRepository.TearDown();
        }
    }
}
