using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models.Authentication;

namespace Castel.Core.Repository
{
    public class AccountRepository : GenericRepository<StoreUser>, IAccountRepository
    {
        public AccountRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
