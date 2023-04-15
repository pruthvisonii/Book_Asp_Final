using NUnit.Framework;
using BookShopAsp.Models;
using Microsoft.AspNetCore.Identity;

namespace BookShopAsp.Tests.Models
{
    [TestFixture]
    public class MyUserTests
    {
        [Test]
        public void MyUser_Orders_DefaultValue_ShouldBeEmptyList()
        {
            // Arrange
            var user = new MyUser();

            // Assert
            Assert.IsEmpty(user.Orders);
        }
    }
}