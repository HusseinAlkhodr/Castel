namespace Castel.Models
{
    public class Division : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual List<Item> Items { get; set; }
        public virtual ICollection<BuyInvoice> BuyInvoices { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoices { get; set; }
    }
}
