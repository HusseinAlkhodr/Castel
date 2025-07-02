using Microsoft.AspNetCore.Identity;

namespace Castel.Models.Authentication
{
    public class StoreRole : IdentityRole<long>
    {
        public StoreRole() : base() { }
        public StoreRole(string roleName) : base(roleName) { }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
