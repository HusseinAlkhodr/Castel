using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class BuyInvoiceRepository : GenericRepository<BuyInvoice>, IBuyInvoiceRepository
    {
        public BuyInvoiceRepository(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
