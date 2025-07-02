using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class PricingRepository : GenericRepository<Pricing>, IPricingRepository
    {
        public PricingRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
