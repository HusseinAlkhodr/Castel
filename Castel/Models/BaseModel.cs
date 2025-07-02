using System.ComponentModel.DataAnnotations.Schema;
using Castel.Models.Authentication;

namespace Castel.Models
{
    public class BaseModel
    {
        public long id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public long? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public StoreUser CreatedBy { get; set; }
        public long? UpdatedById { get; set; }
        [ForeignKey(nameof(UpdatedById))]
        public StoreUser UpdatedBy { get; set; }

    }
}
