using AutoMapper;
using Castel.Core.Interface;
using Castel.Core.Repository;
using Castel.DataBaseContext;

namespace Castel.Core.Unit
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext dbContext;
        private readonly ILoggerFactory logger;
        private readonly ILogger log;
        private readonly IMapper mapper;
        private IAccountRepository accountRepository;
        private IDivisionRepository divisionRepository;
        private IItemRepository itemRepository;
        private IVendorRepository vendorRepository;
        private IPricingRepository priceRepository;
        private IBuyInvoiceRepository buyInvoiceRepository;
        private IBuyInvoiceMasterRepository buyInvoiceMasterRepository;
        private ISaleInvoiceRepository saleInvoiceRepository;
        private ISaleInvoiceMasterRepository saleInvoiceMasterRepository;
        private IExchangeRateRepository exchangeRateRepository;
        public UnitOfWork(StoreDbContext dbContext, ILoggerFactory logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            log = logger.CreateLogger("logs");
            this.mapper = mapper;
        }
        public IAccountRepository AccountRepository =>
           accountRepository ??= new AccountRepository(dbContext, mapper);
        public IDivisionRepository DivisionRepository =>
            divisionRepository ??= new DivisionRepository(dbContext, mapper);
        public IItemRepository ItemRepository =>
            itemRepository ??= new ItemRepository(dbContext, mapper);
        public IVendorRepository VendorRepository =>
            vendorRepository ??= new VendorRepository(dbContext, mapper);
        public IPricingRepository PriceRepository =>
            priceRepository ??= new PricingRepository(dbContext, mapper);

        public IBuyInvoiceRepository BuyInvoiceRepository =>
            buyInvoiceRepository ??= new BuyInvoiceRepository(dbContext, mapper);
        public IBuyInvoiceMasterRepository BuyInvoiceMasterRepository =>
            buyInvoiceMasterRepository ??= new BuyInvoiceMasterRepository(dbContext, mapper);

        public ISaleInvoiceRepository SaleInvoiceRepository =>
            saleInvoiceRepository ??= new SaleInvoiceReposiroty(dbContext, mapper);

        public IExchangeRateRepository ExchangeRateRepository =>
            exchangeRateRepository ??= new ExchangeRateRepository(dbContext, mapper);

        public ISaleInvoiceMasterRepository SaleInvoiceMasterRepository =>
            saleInvoiceMasterRepository ??= new SaleInvoiceMasterRepository(dbContext, mapper);

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }

        public void SaveAsync()
        {
            dbContext.SaveChanges();
        }

    }
}
