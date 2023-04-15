using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using BookShopAsp.Models;
using System;

namespace BookShopAsp.Tests.Models
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void Order_ValidData_ShouldPass()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = "testuser",
                Address = "123 Main St",
                BookId = 1,
                Date = DateTime.Now
            };

            // Act
            var validationContext = new ValidationContext(order);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Order_UserId_IsRequired()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                Address = "123 Main St",
                BookId = 1,
                Date = DateTime.Now
            };

            // Act
            var validationContext = new ValidationContext(order);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(results[0].ErrorMessage, Is.EqualTo("The Buyer field is required."));
        }

        [Test]
        public void Order_Address_IsRequired()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = "testuser",
                BookId = 1,
                Date = DateTime.Now
            };

            // Act
            var validationContext = new ValidationContext(order);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(results[0].ErrorMessage, Is.EqualTo("The Address field is required."));
        }

        [Test]
        public void Order_BookId_IsRequired()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = "testuser",
                Address = "123 Main St",
                Date = DateTime.Now
            };

            // Act
            var validationContext = new ValidationContext(order);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(results[0].ErrorMessage, Is.EqualTo("The Book field is required."));
        }

        [Test]
        public void Order_Date_IsOptional()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = "testuser",
                Address = "123 Main St",
                BookId = 1
            };

            // Act
            var validationContext = new ValidationContext(order);
            var results = new System.Collections.Generic.List<ValidationResult>();
            var isValid = Validator.TryValidateObject(order, validationContext, results, true);

            // Assert
            Assert.That(isValid, Is.True);
        }
    }
}