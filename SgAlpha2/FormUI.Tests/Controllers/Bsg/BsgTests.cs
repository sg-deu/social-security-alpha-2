using FluentAssertions;
using FormUI.Controllers.Bsg;
using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Bsg
{
    [TestFixture]
    public class BsgTests : WebTest
    {
        [Test]
        public void Index_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Index());

                response.Doc.Document.Body.TextContent.Should().Contain("Best Start Grant Alpha");
            });
        }

        [Test]
        public void Overview_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.Overview());

                response.Doc.Document.Body.TextContent.Should().Contain("Overview");
            });
        }

        [Test]
        public void AboutYou_GET()
        {
            WebAppTest(client =>
            {
                var response = client.Get(BsgActions.AboutYou());

                response.Doc.Document.Body.TextContent.Should().Contain("About You");
            });
        }
    }
}
