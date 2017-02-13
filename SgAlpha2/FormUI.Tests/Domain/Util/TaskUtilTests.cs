using System;
using System.Threading.Tasks;
using FormUI.Domain.Util;
using NUnit.Framework;

namespace FormUI.Tests.Domain.Util
{
    [TestFixture]
    public class TaskUtilTests
    {
        [Test]
        public void Await_ReThrows_Exceptions()
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var v1 = 5;
                var v2 = 0;
                Func<int> f = () => v1 / v2;
                TaskUtil.Await(() => Task.Factory.StartNew(f));
            });
        }
    }
}
