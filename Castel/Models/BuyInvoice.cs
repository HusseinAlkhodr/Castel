using System.ComponentModel.DataAnnotations.Schema;

namespace Castel.Models
{
    public class BuyInvoice : BaseModel
    {
        public long ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }
        public long DivisionId { get; set; }
        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }
        public long VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; }
        public long BuyInvoiceMasterId { get; set; }
        public BuyInvoiceMaster BuyInvoiceMaster { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
