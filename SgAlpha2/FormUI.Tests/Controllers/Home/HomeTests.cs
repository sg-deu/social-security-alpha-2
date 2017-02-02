using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Home;
using FormUI.Tests.Controllers.Util;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Home
{
    [TestFixture]
    public class HomeTests : WebTest
    {
        [Test]
        public void Index()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            result.Should().NotBeNull();
        }

        [Test]
        [Explicit("Not working with OWIN yet")]
        public void Index_Web()
        {
            WebAppTest(client =>
            {
                var response = client.Get(HomeActions.Index());

                response.Doc.Document.TextContent.Should().Contain("Welcome to Alpha 2");
            });
        }

        [Test]
        public void Password()
        {
            var controller = new HomeController();

            var result = controller.Password() as ViewResult;

            result.Should().NotBeNull();
            result.ViewName.Should().BeNullOrWhiteSpace();
        }
    }
}
