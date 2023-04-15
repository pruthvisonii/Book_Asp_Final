using Microsoft.AspNetCore.Identity;

namespace BookShopAsp.Models
{
    public class MyUser:IdentityUser
    {
        public List<Order> Orders { get; set; }
    }
}
