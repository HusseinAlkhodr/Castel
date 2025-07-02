using AutoMapper;
using Castel.Core.Interface;
using Castel.DataBaseContext;
using Castel.Models;

namespace Castel.Core.Repository
{
    public class SaleInvoiceReposiroty : GenericRepository<SaleInvoice>, ISaleInvoiceRepository
    {
        public SaleInvoiceReposiroty(StoreDbContext context, IMapper map) : base(context, map)
        {
        }
    }
}
