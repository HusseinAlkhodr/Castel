using Microsoft.AspNetCore.Identity;

namespace Castel.Models.Authentication
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual StoreUser User { get; set; }
        public virtual StoreRole Role { get; set; }
    }
}
