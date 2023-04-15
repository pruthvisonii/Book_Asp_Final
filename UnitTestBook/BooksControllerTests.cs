using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using BookShopAsp.Controllers;
using BookShopAsp.Data;
using BookShopAsp.Models;

namespace BookShopAsp.Tests.Controllers
{
    public class BooksControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private BooksController _controller;

        [SetUp]
        public void Setup()
        {
            // Create a mock DbContext using Moq
            _mockContext = new Mock<ApplicationDbContext>();

            // Initialize the controller with the mock DbContext
            _controller = new BooksController(_mockContext.Object);
        }

        [Test]
        public async Task Create_ValidModelState_RedirectsToIndex()
        {
            // Arrange
            var book = new Book
            {
                Name = "Test Book",
                Author = "Test Author",
                Genre = "Test Genre",
                Price = 9
            };

            // Act
            var result = await _controller.Create(book) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsViewWithModel()
        {
            // Arrange
            var book = new Book
            {
                Name = "Test Book",
                Author = "Test Author",
                Genre = "Test Genre",
                Price = 9 // invalid price
            };
            _controller.ModelState.AddModelError("Price", "Price must be a positive number");

            // Act
            var result = await _controller.Create(book) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(book, result.Model);
        }
    }
}