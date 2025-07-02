namespace Castel.Models
{
    public class SaleInvoiceMaster : BaseModel
    {
        public string ClientName { get; set; }
        public List<SaleInvoice> SaleInvoices { get; set; }
    }
}
