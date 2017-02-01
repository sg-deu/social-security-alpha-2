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

        [Test]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            (result.ViewBag.Message as string).Should().Be("Your application description page.");
        }

        [Test]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            result.Should().NotBeNull();
        }
    }
}
