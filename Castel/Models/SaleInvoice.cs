using System.ComponentModel.DataAnnotations.Schema;

namespace Castel.Models
{
    public enum PaymentType
    {
        USD = 1,
        SYP = 2
    }
    public class SaleInvoice : BaseModel
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
        public long SaleInvoiceMasterId { get; set; }
        public SaleInvoiceMaster SaleInvoiceMaster { get; set; }
        public int Qty { get; set; }
        public int NetAmount { get; set; }
        public int Total { get; set; }
        public PaymentType PaymentType { get; set; }
        public long Dollar { get; set; }
    }
}
