using Castel.Core.Interface;

namespace Castel.Core.Unit
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IDivisionRepository DivisionRepository { get; }
        IItemRepository ItemRepository { get; }
        IVendorRepository VendorRepository { get; }
        IPricingRepository PriceRepository { get; }
        IBuyInvoiceRepository BuyInvoiceRepository { get; }
        IBuyInvoiceMasterRepository BuyInvoiceMasterRepository { get; }
        ISaleInvoiceRepository SaleInvoiceRepository { get; }
        ISaleInvoiceMasterRepository SaleInvoiceMasterRepository { get; }
        IExchangeRateRepository ExchangeRateRepository { get; }
        Task Save();
        void SaveAsync();
    }
}
