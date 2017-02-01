using System.Web.Mvc;
using FluentAssertions;
using FormUI.Controllers.Home;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Home
{
    [TestFixture]
    public class HomeTests
    {
        [Test]
        public void Index()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            result.Should().NotBeNull();
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
