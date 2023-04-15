using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookShopAsp.Models;
namespace BookShopAsp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
      public DbSet<Book>? Books { get; set; }
      public DbSet<Order>? Orders { get; set; }    
    }
}