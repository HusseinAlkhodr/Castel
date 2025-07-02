namespace Castel.Models
{
    public interface ISoftDelete
    {
        public int IsArchived { get; set; }
        public DateTime? ArchiveDate { get; set; }
    }
}
