using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Castel.Models.Authentication
{
    public class StoreUserClaim : IdentityUserClaim<long>
    {
        [ForeignKey(nameof(Id))]
        public StoreUser User { get; set; }
    }
}
