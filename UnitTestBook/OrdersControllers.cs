using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookShopAsp.Controllers;
using BookShopAsp.Data;
using BookShopAsp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BookShopAsp.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private OrdersController _controller;
        private Mock<ApplicationDbContext> _context;
        private Mock<UserManager<IdentityUser>> _userManager;

        [SetUp]
        public void Setup()
        {
            _context = new Mock<ApplicationDbContext>();
            _userManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            _controller = new OrdersController(_context.Object, _userManager.Object);
        }

        [Test]
        public async Task MyOrders_ReturnsViewResult_WithListOfOrders()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "testuser" };
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = "1", BookId = 1 },
                new Order { Id = 2, UserId = "1", BookId = 2 }
            };
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _context.Setup(c => c.Orders.Include(o => o.Book).Include(o => o.User))
                .Returns(MockDbSet(orders).Include(o => o.Book).Include(o => o.User));

            // Act
            var result = await _controller.MyOrders();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<List<Order>>(viewResult.Model);
            var model = viewResult.Model as List<Order>;
            Assert.AreEqual(orders.Count, model.Count);
            Assert.IsTrue(model.All(o => o.UserId == user.Id));
        }

        private static DbSet<T> MockDbSet<T>(List<T> list) where T : class
        {
            IQueryable<T> queryableList = list.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            return mockSet.Object;
        }
    }
}