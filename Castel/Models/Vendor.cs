namespace Castel.Models
{
    public class Vendor : BaseModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual List<Item> Items { get; set; }
        public ICollection<BuyInvoice> BuyInvoices { get; set; }
        public ICollection<SaleInvoice> SaleInvoices { get; set; }
    }
}
