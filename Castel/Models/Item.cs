using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Castel.Models
{
    public enum ItemType
    {
        [Display(Name = "قطعة")]
        Piece = 1,

        [Display(Name = "طرد")]
        Package = 20,

        [Display(Name = "كروز")]
        Cruise = 10
    }

    public class Item : BaseModel
    {
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public int Qty { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double NetAmount { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.Piece;
        public double PriceInDollar { get; set; } = 0;
        public double NetAmountInDollar { get; set; } = 0;
        public virtual ICollection<BuyInvoice> BuyInvoices { get; set; }
        public virtual ICollection<SaleInvoice> SaleInvoices { get; set; }
        public virtual ICollection<Pricing> PriceHistory { get; set; } = new List<Pricing>();
        public long VendorId { get; set; }
        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; }
        public long DivisionId { get; set; }
        [ForeignKey(nameof(DivisionId))]
        public Division Division { get; set; }
    }
}
