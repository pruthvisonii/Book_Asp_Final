using BookShopAsp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BookShopAsp.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
