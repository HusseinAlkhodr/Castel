using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Castel.Models.Authentication
{
    public class StoreUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<StoreUser, StoreRole>
    {
        public StoreUserClaimsPrincipalFactory(UserManager<StoreUser> userManager,
            RoleManager<StoreRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(StoreUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // تحقق من القيم قبل إضافتها لتجنب null
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                identity.AddClaim(new Claim("FirstName", user.FirstName));
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                identity.AddClaim(new Claim("LastName", user.LastName));
            }

            // يمكنك إضافة أي Claims أخرى تحتاجها هنا

            return identity;
        }
    }
}
