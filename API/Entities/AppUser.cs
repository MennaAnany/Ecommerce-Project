using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public List<Order> Orders { get; set; }

    }
}
