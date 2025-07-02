using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class BuyInvoiceMasterRepository : GenericRepository<BuyInvoiceMaster>, IBuyInvoiceMasterRepository
    {
        public BuyInvoiceMasterRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
