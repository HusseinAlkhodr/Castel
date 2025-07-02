using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class SaleInvoiceMasterRepository : GenericRepository<SaleInvoiceMaster>, ISaleInvoiceMasterRepository
    {
        public SaleInvoiceMasterRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
