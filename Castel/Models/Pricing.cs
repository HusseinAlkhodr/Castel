using System.ComponentModel.DataAnnotations.Schema;

namespace Castel.Models
{
    public class Pricing : BaseModel
    {
        public long ItemId { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }
        public double Old_Price { get; set; } = 0;
        public double New_Price { get; set; }
        public double Old_NetAmount { get; set; } = 0;
        public double New_NetAmount { get; set; }
    }
}
