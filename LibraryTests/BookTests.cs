using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using BookShopAsp.Models;

namespace BookShopAsp.Tests.Models
{
    [TestFixture]
    public class BookTests
    {
        [Test]
        public void Book_ValidData_ShouldPass()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                Genre = "Classic",
                Price = 10
            };

            // Act
            var validationContext = new ValidationContext(book);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(book, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.True);
        }
    }
}