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
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }
    }
}
