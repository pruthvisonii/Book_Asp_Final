using BookShopAsp.Controllers;
using BookShopAsp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopAsp.Tests.Controllers
{
    [TestFixture]
    public class RolesControllerTests
    {
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private Mock<UserManager<IdentityUser>> _mockUserManager;

        private RolesController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            _controller = new RolesController(_mockRoleManager.Object, _mockUserManager.Object);
        }

        [Test]
        public void Index_ReturnsViewWithRoles()
        {
            // Arrange
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin" },
                new IdentityRole { Id = "2", Name = "User" }
            };

            _mockRoleManager.Setup(rm => rm.Roles).Returns(roles.AsQueryable());

            // Act
            var result = _controller.Index() as ViewResult;
            var model = result.Model as List<IdentityRole>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(model.Count, Is.EqualTo(2));
            Assert.That(model[0].Name, Is.EqualTo("Admin"));
            Assert.That(model[1].Name, Is.EqualTo("User"));
        }

        [Test]
        public async Task Create_WithValidModel_ReturnsIndex()
        {
            // Arrange
            var role = "Admin";
            var identityResult = IdentityResult.Success;

            _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(identityResult);

            // Act
            var result = await _controller.Create(role) as RedirectToActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }
    }
}
