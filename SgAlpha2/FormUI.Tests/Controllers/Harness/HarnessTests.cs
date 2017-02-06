using System.Net;
using FluentAssertions;
using FormUI.Controllers.Harness;
using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Harness
{
    [TestFixture]
    public class HarnessTests : WebTest
    {
        [Test]
        public void Index_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.Index());
                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            });
        }

        [Test]
        public void InputText_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HarnessActions.InputText());
                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            });
        }
    }
}
